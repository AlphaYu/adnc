using Adnc.Demo.Remote.Event;

namespace Adnc.Demo.Ord.Application.Services.Implements;

/// <summary>
///  订单管理
/// </summary>
/// <param name="orderRepo"></param>
/// <param name="orderMgr"></param>
/// <param name="whseClient"></param>
/// <param name="adminClient"></param>
public class OrderService(IEfBasicRepository<Order> orderRepo, OrderManager orderMgr, IWhseRestClient whseClient, IAdminRestClient adminClient)
    : AbstractAppService, IOrderService
{
    /// <summary>
    /// 创建订单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<OrderDto> CreateAsync(OrderCreationDto input)
    {
        input.TrimStringFields();
        var productIds = input.Items.Select(x => x.ProductId).ToArray();
        //调用whse服务获取产品的价格,名字
        var products = await whseClient.GetProductsAsync(new ProductSearchRequest { Ids = productIds }) ?? [];
        var orderId = IdGenerater.GetNextId();
        var items = from o in input.Items
                    join p in products on o.ProductId equals p.Id
                    select (new OrderItemProduct(p.Id, p.Name, p.Price), o.Count);

        //需要发布领域事件,通知仓储中心冻结库存
        var order = await orderMgr.CreateAsync
                                    (orderId
                                    , input.CustomerId
                                    , items
                                    , new OrderReceiver(input.DeliveryInfomaton.Name, input.DeliveryInfomaton.Phone, input.DeliveryInfomaton.Address)
                                    );
        // 保存到数据库
        await orderRepo.InsertAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// 标记订单状态
    /// </summary>
    /// <param name="eventDto"></param>
    /// <param name="tracker"></param>
    /// <returns></returns>
    public async Task MarkCreatedStatusAsync(WarehouseQtyBlockedEvent eventDto, IMessageTracker tracker)
    {
        eventDto.TrimStringFields();
        var order = await orderRepo.GetRequiredAsync(eventDto.OrderId);
        order.MarkCreatedStatus(eventDto.IsSuccess, eventDto.Remark);

        await orderRepo.UpdateAsync(order);
        await tracker.MarkAsProcessedAsync(eventDto);
    }

    /// <summary>
    /// 修改订单信息
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
    /// 删除订单
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
    /// 订单付款
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> PayAsync(long id)
    {
        var order = await orderRepo.GetRequiredAsync(id);

        //需要发布领域事件，客户中心订阅该事件
        await orderMgr.PayAsync(order);

        await orderRepo.UpdateAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> CancelAsync(long id)
    {
        var order = await orderRepo.GetRequiredAsync(id);

        //需要发布领域事件，仓储中心订阅该事件
        await orderMgr.CancelAsync(order);

        await orderRepo.UpdateAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// 获取订单信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> GetAsync(long id)
    {
        var order = await orderRepo.GetRequiredAsync(id, x => x.Items);
        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// 订单分页列表
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
            //调用admin微服务获取字典,组合订单状态信息
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
