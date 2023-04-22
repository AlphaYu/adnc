namespace Adnc.Shared.Rpc.Event;

/// <summary>
/// 订单创建事件
/// </summary>
[Serializable]
public class OrderCreatedEvent : EventEntity
{
    public OrderCreatedEvent()
    {
    }

    public OrderCreatedEvent(long id, string eventSource,long orderId, IEnumerable<OrderItem> orderItems)
        : base(id, eventSource)
    {
        OrderId = orderId;
        Products = orderItems;
    }

    public long OrderId { get; set; }
    public IEnumerable<OrderItem> Products { get; set; } = new List<OrderItem>();

    public class OrderItem
    {
        public long ProductId { get; set; }
        public int Qty { get; set; }
    }
}