namespace Adnc.Shared.Rpc.Event;

/// <summary>
/// 订单支付事件
/// </summary>
[Serializable]
public sealed class OrderPaidEvent : EventEntity<OrderPaidEvent.EventData>
{
    public OrderPaidEvent()
    {
    }

    public OrderPaidEvent(long id, EventData eventData, string eventSource)
        : base(id, eventData, eventSource)
    {
    }

    public class EventData
    {
        public long OrderId { get; set; }

        public long CustomerId { get; set; }

        public decimal Amount { get; set; }
    }
}