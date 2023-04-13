using Adnc.Demo.Shared.Rpc.Http.Services;

namespace Adnc.Demo.Ord.Application.Services.Implements;

/// <summary>
/// 订单管理
/// </summary>
public class OrderAppService : AbstractAppService, IOrderAppService
{
    private readonly OrderManager _orderMgr;
    private readonly IEfBasicRepository<Order> _orderRepo;
    private readonly IWhseRestClient _whseRestClient;
    private readonly IMaintRestClient _maintRestClient;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="orderRepo"></param>
    /// <param name="orderMgr"></param>
    /// <param name="whseRestClient"></param>
    /// <param name="maintRestClient"></param>
    public OrderAppService(
         IEfBasicRepository<Order> orderRepo
        , OrderManager orderMgr
        , IWhseRestClient whseRestClient
        , IMaintRestClient maintRestClient)
    {
        _orderRepo = orderRepo;
        _orderMgr = orderMgr;
        _whseRestClient = whseRestClient;
        _maintRestClient = maintRestClient;
    }

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
        var restRpcResult = await _whseRestClient.GetProductsAsync(new ProductSearchListRto { Ids = productIds });
        Guard.Checker.ThrowIf(() => !restRpcResult.IsSuccessStatusCode || restRpcResult.Content.IsNullOrEmpty(), "product is not extists");
        var products = restRpcResult.Content;
        var orderId = IdGenerater.GetNextId();
        var items = from o in input.Items
                    join p in products on o.ProductId equals p.Id
                    select (new OrderItemProduct(p.Id, p.Name, p.Price), o.Count);

        //需要发布领域事件,通知仓储中心冻结库存
        var order = await _orderMgr.CreateAsync
                                    (orderId
                                    , input.CustomerId
                                    , items
                                    , new OrderReceiver(input.DeliveryInfomaton?.Name, input.DeliveryInfomaton?.Phone, input.DeliveryInfomaton?.Address)
                                    );
        // 保存到数据库
        await _orderRepo.InsertAsync(order);

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
        var order = await _orderRepo.GetAsync(eventDto.OrderId);
        order.MarkCreatedStatus(eventDto.IsSuccess, eventDto.Remark);

        await _orderRepo.UpdateAsync(order);
        await tracker?.MarkAsProcessedAsync(eventDto);
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

        var order = await _orderRepo.GetAsync(id);

        order.ChangeReceiver(new OrderReceiver(
            input.DeliveryInfomaton.Name
            , input.DeliveryInfomaton.Phone
            , input.DeliveryInfomaton.Address)
        );

        await _orderRepo.UpdateAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// 删除订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        var order = await _orderRepo.GetAsync(id);
        order.MarkDeletedStatus(string.Empty);

        await _orderRepo.UpdateAsync(order);
    }

    /// <summary>
    /// 订单付款
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> PayAsync(long id)
    {
        var order = await _orderRepo.GetAsync(id);

        //需要发布领域事件，客户中心订阅该事件
        await _orderMgr.PayAsync(order);

        await _orderRepo.UpdateAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> CancelAsync(long id)
    {
        var order = await _orderRepo.GetAsync(id);

        //需要发布领域事件，仓储中心订阅该事件
        await _orderMgr.CancelAsync(order);

        await _orderRepo.UpdateAsync(order);

        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// 获取订单信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> GetAsync(long id)
    {
        var order = await _orderRepo.GetAsync(id, x => x.Items);
        return Mapper.Map<OrderDto>(order);
    }

    /// <summary>
    /// 订单分页列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public async Task<PageModelDto<OrderDto>> GetPagedAsync(OrderSearchPagedDto search)
    {
        search.TrimStringFields();
        var whereCondition = ExpressionCreator
                                            .New<Order>()
                                            .AndIf(search.Id > 0, x => x.Id == search.Id);

        var total = await _orderRepo.CountAsync(whereCondition);
        if (total == 0)
            return new PageModelDto<OrderDto>(search);

        var entities = _orderRepo
                                .Where(whereCondition)
                                .OrderByDescending(x => x.Id)
                                .Skip(search.SkipRows())
                                .Take(search.PageSize)
                                .ToListAsync();
        var orderDtos = Mapper.Map<List<OrderDto>>(entities);
        if (orderDtos.IsNotNullOrEmpty())
        {
            //调用maint微服务获取字典,组合订单状态信息
            var restRpcResult = await _maintRestClient.GetDictAsync(ServiceAddressConsts.OrderStatusId);
            if (restRpcResult.IsSuccessStatusCode)
            {
                var dict = restRpcResult.Content;
                if (dict is not null && dict.Children.IsNotNullOrEmpty())
                {
                    orderDtos.ForEach(x =>
                    {
                        x.StatusChangesReason = dict.Children.FirstOrDefault(d => d.Name == x.StatusCode.ToSafeString())?.Name;
                    });
                }
            }
        }

        return new PageModelDto<OrderDto>(search, orderDtos, total);
    }
}