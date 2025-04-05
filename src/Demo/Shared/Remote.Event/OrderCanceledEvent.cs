using Adnc.Shared.Remote.Event;

namespace Adnc.Demo.Remote.Event;

/// <summary>
/// 订单取消事件
/// </summary>
[Serializable]
public sealed class OrderCanceledEvent : EventEntity
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
