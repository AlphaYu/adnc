using Adnc.Demo.Ord.Application.Contracts.Dtos.Order;
using Adnc.Infra.EventBus.Tracker;

namespace Adnc.Demo.Ord.Application.Services;

/// <summary>
/// Order management
/// </summary>
/// <param name="orderRepo"></param>
/// <param name="orderMgr"></param>
/// <param name="whseClient"></param>
/// <param name="adminClient"></param>
public class OrderService(IEfBasicRepository<Order> orderRepo, OrderManager orderMgr, IWhseRestClient whseClient, IAdminRestClient adminClient)
    : AbstractAppService, IOrderService
{
    /// <summary>
    /// Create an order
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<OrderDto> CreateAsync(OrderCreationDto input)
    {
        input.TrimStringFields();
        var productIds = input.Items.Select(x => x.ProductId).ToArray();
        // Call the whse service to get product prices and names.
        var products = await whseClient.GetProductsAsync(new ProductSearchRequest { Ids = productIds }) ?? [];
        var orderId = IdGenerater.GetNextId();
        var items = from o in input.Items
                    join p in products on o.ProductId equals p.Id
                    select (new OrderItemProduct(p.Id, p.Name, p.Price), o.Count);

        // A domain event needs to be published to notify the warehouse center to reserve inventory.
        var order = await orderMgr.CreateAsync
                                    (orderId
                                    , input.CustomerId
                                    , items
                                    , new OrderReceiver(input.DeliveryInfomaton.Name, input.DeliveryInfomaton.Phone, input.DeliveryInfomaton.Address)
                                    );
        // Save to the database.
        await orderRepo.InsertAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// Mark the order status
    /// </summary>
    /// <param name="eventDto"></param>
    /// <param name="tracker"></param>
    /// <returns></returns>
    public async Task MarkCreatedStatusAsync(Remote.Event.WarehouseQtyBlockedEvent eventDto, IMessageTracker tracker)
    {
        eventDto.TrimStringFields();
        var order = await orderRepo.GetRequiredAsync(eventDto.OrderId);
        order.MarkCreatedStatus(eventDto.IsSuccess, eventDto.Remark);

        await orderRepo.UpdateAsync(order);
        await tracker.MarkAsProcessedAsync(eventDto);
    }

    /// <summary>
    /// Update order information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<OrderDto> UpdateAsync(long id, OrderUpdationDto input)
    {
        input.TrimStringFields();

        var order = await orderRepo.GetRequiredAsync(id);

        order.ChangeReceiver(new OrderReceiver(
            input.DeliveryInfomaton.Name
            , input.DeliveryInfomaton.Phone
            , input.DeliveryInfomaton.Address)
        );

        await orderRepo.UpdateAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// Delete an order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        var order = await orderRepo.GetRequiredAsync(id);
        order.MarkDeletedStatus(string.Empty);

        await orderRepo.UpdateAsync(order);
    }

    /// <summary>
    /// Pay for an order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> PayAsync(long id)
    {
        var order = await orderRepo.GetRequiredAsync(id);

        // A domain event needs to be published, and the customer center subscribes to it.
        await orderMgr.PayAsync(order);

        await orderRepo.UpdateAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// Cancel an order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> CancelAsync(long id)
    {
        var order = await orderRepo.GetRequiredAsync(id);

        // A domain event needs to be published, and the warehouse center subscribes to it.
        await orderMgr.CancelAsync(order);

        await orderRepo.UpdateAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// Get order information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> GetAsync(long id)
    {
        var order = await orderRepo.GetRequiredAsync(id, x => x.Items);
        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// Get a paginated order list
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PageModelDto<OrderDto>> GetPagedAsync(OrderSearchPagedDto input)
    {
        input.TrimStringFields();
        var whereCondition = ExpressionCreator
                                            .New<Order>()
                                            .AndIf(input.Id > 0, x => x.Id == input.Id);

        var total = await orderRepo.CountAsync(whereCondition);
        if (total == 0)
        {
            return new PageModelDto<OrderDto>(input);
        }

        var entities = orderRepo
                                .Where(whereCondition)
                                .OrderByDescending(x => x.Id)
                                .Skip(input.SkipRows())
                                .Take(input.PageSize)
                                .ToListAsync();
        var orderDtos = Mapper.Map<List<OrderDto>>(entities);
        if (orderDtos.IsNotNullOrEmpty())
        {
            // Call the admin microservice to get dictionary data and compose order status information.
            var orderStatus = (await adminClient.GetDictOptionsAsync("order_status")).FirstOrDefault();
            if (orderStatus is not null)
            {
                orderDtos.ForEach(x =>
                {
                    x.StatusChangesReason = orderStatus.DictDataList.FirstOrDefault(d => d.Value == x.StatusCode.ToString())?.Label ?? string.Empty; ;
                });
            }
        }

        return new PageModelDto<OrderDto>(input, orderDtos, total);
    }
}
