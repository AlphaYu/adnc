using Adnc.Infra.EventBus;

namespace Adnc.Demo.Remote.Event;

/// <summary>
/// Order Created Event
/// </summary>
[Serializable]
public class OrderCreatedEvent : BaseEvent
{
    public OrderCreatedEvent()
    {
    }

    public OrderCreatedEvent(long id, string eventSource, long orderId, IEnumerable<OrderItem> orderItems)
        : base(id, eventSource)
    {
        OrderId = orderId;
        Products = orderItems;
    }

    public long OrderId { get; set; }
    public IEnumerable<OrderItem> Products { get; set; } = [];

    public class OrderItem
    {
        public long ProductId { get; set; }
        public int Qty { get; set; }
    }
}
