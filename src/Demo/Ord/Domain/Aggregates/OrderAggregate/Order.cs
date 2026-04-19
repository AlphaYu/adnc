namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

/// <summary>
/// Order
/// </summary>
public class Order : AggregateRootWithBasicAuditInfo
{
    public static readonly int Remark_MaxLength = 64;
    public static readonly int ChangesReason_MaxLength = 32;
    public static readonly int Name_MaxLength = 16;
    public static readonly int Phone_MaxLength = 11;
    public static readonly int Address_MaxLength = 64;

    private Order()
    {
        Status = default!;
        Receiver = default!;
        Items = Array.Empty<OrderItem>();
    }

    internal Order(long id, long customerId, OrderReceiver orderReceiver, string? remark = null)
    {
        Id = Checker.Variable.GTZero(id, nameof(id));
        CustomerId = Checker.Variable.GTZero(customerId, nameof(customerId));
        Items = [];
        Receiver = Checker.Variable.NotNull(orderReceiver, nameof(orderReceiver));
        Status = new OrderStatus(OrderStatusCodes.Creating);
        Remark = remark;
        Amount = 0;
    }

    /// <summary>
    /// Customer ID
    /// </summary>
    public long CustomerId { get; private set; }

    /// <summary>
    /// Order amount
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// Receiver information
    /// </summary>
    public OrderReceiver Receiver { get; private set; }

    /// <summary>
    /// Order items
    /// </summary>
    public virtual ICollection<OrderItem> Items { get; private set; }

    /// <summary>
    /// Add an order product
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="product"></param>
    /// <param name="count"></param>
    public void AddProduct(long itemId, OrderItemProduct product, int count)
    {
        Checker.Variable.NotNull(product, nameof(product));
        Checker.Variable.GTZero(count, nameof(count));

        var existProduct = Items.FirstOrDefault(x => x.Product.Id == product.Id);
        if (existProduct is null)
        {
            Items.Add(new OrderItem(itemId, Id, product, count));
        }
        else
        {
            existProduct.ChangeCount(count);
        }

        Amount += product.Price * count;
    }

    /// <summary>
    /// Update receiver information
    /// </summary>
    /// <param name="orderReceiver"></param>
    public void ChangeReceiver(OrderReceiver orderReceiver)
    {
        Checker.Variable.NotNull(orderReceiver, nameof(orderReceiver));
        Receiver = orderReceiver;
    }

    /// <summary>
    /// Mark the order creation status
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="changesReason"></param>
    public void MarkCreatedStatus(bool isSuccess, string changesReason)
    {
        var status = isSuccess ? OrderStatusCodes.WaitPay : OrderStatusCodes.Canceling;
        ChangeStatus(status, changesReason);
    }

    /// <summary>
    /// Mark the order deletion status
    /// </summary>
    /// <param name="changesReason"></param>
    public void MarkDeletedStatus(string changesReason)
    {
        ChangeStatus(OrderStatusCodes.Deleted, changesReason);
    }

    /// <summary>
    /// Change the order status
    /// </summary>
    /// <param name="newStatusCode"></param>
    /// <param name="changesReason"></param>
    internal void ChangeStatus(OrderStatusCodes newStatusCode, string changesReason)
    {
        Status = newStatusCode switch
        {
            OrderStatusCodes.Canceling when Status.Code == OrderStatusCodes.WaitPay => new OrderStatus(newStatusCode, changesReason),
            OrderStatusCodes.Cancelled when Status.Code == OrderStatusCodes.Canceling => new OrderStatus(newStatusCode, changesReason),
            OrderStatusCodes.Deleted when Status.Code == OrderStatusCodes.Cancelled => new OrderStatus(newStatusCode, changesReason),
            OrderStatusCodes.Paying when Status.Code == OrderStatusCodes.WaitPay => new OrderStatus(newStatusCode, changesReason),
            OrderStatusCodes.WaitSend when Status.Code == OrderStatusCodes.Paying => new OrderStatus(newStatusCode, changesReason),
            OrderStatusCodes.WaitConfirm when Status.Code == OrderStatusCodes.WaitSend => new OrderStatus(newStatusCode, changesReason),
            OrderStatusCodes.WaitRate when Status.Code == OrderStatusCodes.WaitConfirm => new OrderStatus(newStatusCode, changesReason),
            OrderStatusCodes.Finished when Status.Code == OrderStatusCodes.WaitRate => new OrderStatus(newStatusCode, changesReason),
            _ => throw new ArgumentException("Status change is not allowed", nameof(newStatusCode)),
        };
    }
}
