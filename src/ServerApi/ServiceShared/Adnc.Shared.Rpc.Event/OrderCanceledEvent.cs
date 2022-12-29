namespace Adnc.Shared.Rpc.Event;

/// <summary>
/// 订单取消事件
/// </summary>
[Serializable]
public sealed class OrderCanceledEvent : EventEntity<OrderCanceledEvent.EventData>
{
    public OrderCanceledEvent()
    {
    }

    public OrderCanceledEvent(long id, EventData eventData, string eventSource)
        : base(id, eventData, eventSource)
    {
    }

    public class EventData
    {
        public long OrderId { get; set; }
    }
}