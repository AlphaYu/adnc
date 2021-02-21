using Adnc.Core.Shared.Events;

namespace Adnc.Orders.Domain.Events
{
    public sealed class OrderCanceledEvent : BaseEvent
    {
        public OrderCanceledEvent(long id, EventData eventData)
            : base(id, eventData)
        {
        }

        public class EventData
        {
            public long OrderId { get; set; }
        }
    }
}
