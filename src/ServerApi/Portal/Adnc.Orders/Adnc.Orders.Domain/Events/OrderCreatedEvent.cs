using Adnc.Infr.EventBus;
using System.Collections.Generic;

namespace Adnc.Orders.Domain.Events
{
    /// <summary>
    /// 订单创建事件
    /// </summary>
    public sealed class OrderCreatedEvent : BaseEvent<OrderCreatedEvent.EventData>
    {
        public OrderCreatedEvent(long id, EventData eventData,string eventSource)
            : base(id, eventData,eventSource)
        {
        }

        public class EventData
        {
            public long OrderId { get; set; }

            public ICollection<(long ProductId, int Qty)> Products { get; set; }
        }
    }
}