namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

/// <summary>
/// 订单
/// </summary>
public class Order : AggregateRootWithBasicAuditInfo
{
    /// <summary>
    /// 客户Id
    /// </summary>
    public long CustomerId { get; private set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// 收货信息
    /// </summary>
    public OrderReceiver Receiver { get; private set; }

    /// <summary>
    /// 订单子项
    /// </summary>
    public virtual ICollection<OrderItem> Items { get; private set; }

    private Order()
    {
        Status = default!;
        Receiver = default!;
        Items = Array.Empty<OrderItem>();
    }

    internal Order(long id, long customerId, OrderReceiver orderReceiver, string? remark = null)
    {
        Id = Guard.Checker.GTZero(id, nameof(id));
        CustomerId = Guard.Checker.GTZero(customerId, nameof(customerId));
        Items = new List<OrderItem>();
        Receiver = Guard.Checker.NotNull(orderReceiver, nameof(orderReceiver));
        Status = new OrderStatus(OrderStatusCodes.Creating);
        Remark = remark;
        Amount = 0;
    }

    /// <summary>
    /// 添加订单产品
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="product"></param>
    /// <param name="count"></param>
    public void AddProduct(long itemId, OrderItemProduct product, int count)
    {
        Guard.Checker.NotNull(product, nameof(product));
        Guard.Checker.GTZero(count, nameof(count));

        var existProduct = Items.FirstOrDefault(x => x.Product.Id == product.Id);
        if (existProduct is null)
            Items.Add(new OrderItem(itemId, Id, product, count));
        else
            existProduct.ChangeCount(count);

        Amount += product.Price * count;
    }

    /// <summary>
    /// 调整收货信息
    /// </summary>
    /// <param name="orderReceiver"></param>
    public void ChangeReceiver(OrderReceiver orderReceiver)
    {
        Guard.Checker.NotNull(orderReceiver, nameof(orderReceiver));
        Receiver = orderReceiver;
    }

    /// <summary>
    /// 标记订单创建状态
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="changesReason"></param>
    public void MarkCreatedStatus(bool isSuccess, string changesReason)
    {
        var status = isSuccess ? OrderStatusCodes.WaitPay : OrderStatusCodes.Canceling;
        ChangeStatus(status, changesReason);
    }

    /// <summary>
    /// 标记订单删除状态
    /// </summary>
    /// <param name="changesReason"></param>
    public void MarkDeletedStatus(string changesReason)
    {
        ChangeStatus(OrderStatusCodes.Deleted, changesReason);
    }

    /// <summary>
    /// 调整订单状态
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
            _ => throw new ArgumentException("不允许修改状态", nameof(newStatusCode)),
        };
    }
}