namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

/// <summary>
/// 订单条目
/// </summary>
public class OrderItem : DomainEntity
{
    /// <summary>
    /// 订单Id
    /// </summary>
    public long OrderId { get; private set; }

    /// <summary>
    /// 产品
    /// </summary>
    public OrderItemProduct Product { get; private set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int Count { get; private set; }

    private OrderItem()
    {
        Product = default!;
    }

    internal OrderItem(long id, long orderId, OrderItemProduct product, int count)
    {
        this.Id = id;
        this.OrderId = Guard.Checker.GTZero(orderId, nameof(orderId));
        this.Product = Guard.Checker.NotNull(product, nameof(product));
        this.Count = Guard.Checker.GTZero(count, nameof(count));
    }

    internal void ChangeCount(int count)
    {
        Guard.Checker.GTZero(count, nameof(count));
        this.Count += count;
    }
}