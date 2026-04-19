using Adnc.Infra.EventBus;

namespace Adnc.Demo.Remote.Event;

/// <summary>
/// Order Paid Event
/// </summary>
[Serializable]
public sealed class OrderPaidEvent : BaseEvent
{
    public OrderPaidEvent()
    {
    }

    public OrderPaidEvent(long id, string eventSource, long orderId, long custmerId, decimal amout)
        : base(id, eventSource)
    {
        OrderId = orderId;
        CustomerId = custmerId;
        Amount = amout;
    }

    public long OrderId { get; init; }

    public long CustomerId { get; init; }

    public decimal Amount { get; init; }
}
