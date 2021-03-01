using Adnc.Infr.EventBus;

namespace Adnc.Ord.Domain.Events
{
    /// <summary>
    /// 订单取消事件
    /// </summary>
    public sealed class OrderCanceledEvent : BaseEvent<OrderCanceledEvent.EventData>
    {
        public OrderCanceledEvent(long id, EventData eventData,string eventSource)
            : base(id, eventData, eventSource)
        {
        }

        public class EventData
        {
            public long OrderId { get; set; }
        }
    }
}
