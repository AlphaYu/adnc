using Adnc.Shared.Rpc.Event;

namespace Adnc.Demo.Ord.Domain.Services;

/// <summary>
/// 订单领域服务
/// </summary>
public class OrderManager : IDomainService
{
    private readonly IEfBasicRepository<Order> _orderRepo;

    public OrderManager(IEfBasicRepository<Order> orderRepo) => _orderRepo = orderRepo;

    /// <summary>
    /// 订单创建
    /// </summary>
    /// <param name="id"></param>
    /// <param name="customerId"></param>
    ///  <param name="items"></param>
    /// <param name="deliveryInfomation"></param>
    /// <param name="remark"></param>
    /// <returns></returns>
    public virtual async Task<Order> CreateAsync(long id, long customerId
        , IEnumerable<(OrderItemProduct Product, int Count)> items
        , OrderReceiver deliveryInfomation
        , string? remark = null)
    {
        var order = new Order(
            id
           , customerId
           , deliveryInfomation
           , remark
        );

        //AddProduct会判断是否有重复的产品
        foreach (var (product, count) in items)
        {
            order.AddProduct(IdGenerater.GetNextId(), new OrderItemProduct(product.Id, product.Name, product.Price), count);
        }

        //发送OrderCreatedEvent事件，通知仓储中心冻结库存
        var products = order.Items.Select(x => new OrderCreatedEvent.OrderItem { ProductId = x.Product.Id, Qty = x.Count });
        var orderCreatedEvent = new OrderCreatedEvent
        {
            Id = IdGenerater.GetNextId(),
            EventSource = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty,
            OrderId = order.Id,
            Products = products
        };
        await order.EventPublisher.Value.PublishAsync(orderCreatedEvent);
        return order;
    }

    /// <summary>
    /// 订单取消，没有付款的订单可以取消
    /// </summary>
    /// <returns></returns>
    public virtual async Task CancelAsync(Order order)
    {
        Guard.Checker.NotNull(order, nameof(order));

        order.ChangeStatus(OrderStatusCodes.Canceling, string.Empty);

        //发布领域事件，通知仓储中心解冻被冻结的库存
        var orderCanceledEvent = new OrderCanceledEvent
        {
            Id = IdGenerater.GetNextId(),
            EventSource = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty,
            OrderId = order.Id,
        };
        await order.EventPublisher.Value.PublishAsync(orderCanceledEvent);
    }

    /// <summary>
    /// 订单付款
    /// </summary>
    /// <returns></returns>
    public virtual async Task PayAsync(Order order)
    {
        Guard.Checker.NotNull(order, nameof(order));

        order.ChangeStatus(OrderStatusCodes.Paying, string.Empty);

        //发布领域事件，通知客户中心扣款(Demo是从余额中扣款)
        var orderPaidEvent = new OrderPaidEvent
        {
            Id = IdGenerater.GetNextId(),
            EventSource = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty,
            OrderId = order.Id,
            Amount = order.Amount,
            CustomerId = order.CustomerId
        };
        await order.EventPublisher.Value.PublishAsync(orderPaidEvent);
    }
}