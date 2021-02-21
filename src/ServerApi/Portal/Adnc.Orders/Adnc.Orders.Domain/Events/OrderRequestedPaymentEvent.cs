using Adnc.Core.Shared.Events;

namespace Adnc.Orders.Domain.Events
{
    public sealed class OrderRequestedPaymentEvent : BaseEvent
    {
        public OrderRequestedPaymentEvent(long id, EventData eventData)
            : base(id, eventData)
        {
        }

        public class EventData
        {
            public long OrderId { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
