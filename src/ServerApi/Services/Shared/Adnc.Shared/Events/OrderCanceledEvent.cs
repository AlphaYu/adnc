using Adnc.Infra.EventBus;
using System;

namespace Adnc.Shared.Events
{
    /// <summary>
    /// 订单取消事件
    /// </summary>
    [Serializable]
    public sealed class OrderCanceledEvent : BaseEvent<OrderCanceledEvent.EventData>
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
}