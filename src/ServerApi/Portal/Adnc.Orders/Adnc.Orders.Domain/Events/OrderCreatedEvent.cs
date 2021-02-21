using Adnc.Core.Shared.Events;
using System.Collections.Generic;

namespace Adnc.Orders.Domain.Events
{
    public sealed class OrderCreatedEvent : BaseEvent
    {
        public OrderCreatedEvent(long id, EventData eventData)
            : base(id, eventData)
        {
        }

        public class EventData
        {
            public long OrderId { get; set; }

            public ICollection<(long ProductId, int Qty)> Products { get; set; }
        }
    }
}