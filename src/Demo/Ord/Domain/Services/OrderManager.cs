using Adnc.Demo.Remote.Event;

namespace Adnc.Demo.Ord.Domain.Services;

/// <summary>
/// Order domain service
/// </summary>
public class OrderManager() : IDomainService
{
    /// <summary>
    /// Create an order
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

        // AddProduct checks whether a duplicate product already exists.
        foreach (var (product, count) in items)
        {
            order.AddProduct(IdGenerater.GetNextId(), new OrderItemProduct(product.Id, product.Name, product.Price), count);
        }

        // Publish the OrderCreatedEvent to notify the warehouse center to reserve inventory.
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
    /// Cancel an order. Unpaid orders can be cancelled.
    /// </summary>
    /// <returns></returns>
    public virtual async Task CancelAsync(Order order)
    {
        Checker.Variable.NotNull(order, nameof(order));

        order.ChangeStatus(OrderStatusCodes.Canceling, string.Empty);

        // Publish a domain event to notify the warehouse center to release reserved inventory.
        var orderCanceledEvent = new OrderCanceledEvent
        {
            Id = IdGenerater.GetNextId(),
            EventSource = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty,
            OrderId = order.Id,
        };
        await order.EventPublisher.Value.PublishAsync(orderCanceledEvent);
    }

    /// <summary>
    /// Pay for an order
    /// </summary>
    /// <returns></returns>
    public virtual async Task PayAsync(Order order)
    {
        Checker.Variable.NotNull(order, nameof(order));

        order.ChangeStatus(OrderStatusCodes.Paying, string.Empty);

        // Publish a domain event to notify the customer center to deduct funds (the demo uses account balance).
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
