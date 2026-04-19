using Adnc.Infra.EventBus;

namespace Adnc.Demo.Remote.Event;

/// <summary>
/// Order Cancelled Event
/// </summary>
[Serializable]
public sealed class OrderCanceledEvent : BaseEvent
{
    public OrderCanceledEvent()
    {
    }

    public OrderCanceledEvent(long id, string eventSource, long orderId)
        : base(id, eventSource)
    {
        OrderId = orderId;
    }

    public long OrderId { get; set; }
}
