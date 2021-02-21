using Adnc.Core.Shared.Events;

namespace Adnc.Warehouse.Domain.Events
{
    public class ShelfToProductAllocatedEvent : BaseEvent
    {

        public ShelfToProductAllocatedEvent(long id, EventData eventData)
            : base(id, eventData)
        {
        }

        public class EventData
        {
            public long ShelfId { get; set; }

            public long ProductId { get; set; }
        }
    }
}
