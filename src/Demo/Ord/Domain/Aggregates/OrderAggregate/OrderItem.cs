namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

/// <summary>
/// Order item
/// </summary>
public class OrderItem : DomainEntity
{
    public static readonly int Name_MaxLength = 64;

    private OrderItem()
    {
        Product = default!;
    }

    internal OrderItem(long id, long orderId, OrderItemProduct product, int count)
    {
        Id = id;
        OrderId = Checker.Variable.GTZero(orderId, nameof(orderId));
        Product = Checker.Variable.NotNull(product, nameof(product));
        Count = Checker.Variable.GTZero(count, nameof(count));
    }

    /// <summary>
    /// Order ID
    /// </summary>
    public long OrderId { get; private set; }

    /// <summary>
    /// Product
    /// </summary>
    public OrderItemProduct Product { get; private set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public int Count { get; private set; }

    internal void ChangeCount(int count)
    {
        Checker.Variable.GTZero(count, nameof(count));
        Count += count;
    }
}
