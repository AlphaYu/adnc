using Adnc.Infra.EventBus;

namespace Adnc.Cus.Core.Events
{
    public class CustomerRechargedEvent : BaseEvent<CustomerRechargedEvent.EventData>
    {
        public CustomerRechargedEvent(long id, EventData eventData, string source)
            : base(id, eventData, source)
        {
        }

        public class EventData
        {
            public long CustomerId { get; set; }

            public decimal Amount { get; set; }

            public long TransactionLogId { get; set; }
        }
    }
}