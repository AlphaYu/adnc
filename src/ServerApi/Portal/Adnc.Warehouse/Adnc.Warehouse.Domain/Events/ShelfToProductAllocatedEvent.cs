using Adnc.Infr.EventBus;

namespace Adnc.Warehouse.Domain.Events
{
    public class ShelfToProductAllocatedEvent : BaseEvent<ShelfToProductAllocatedEvent.EventData>
    {

        public ShelfToProductAllocatedEvent(long id, EventData eventData, string source)
            : base(id, eventData, source)
        {
        }

        public class EventData
        {
            public long ShelfId { get; set; }

            public long ProductId { get; set; }
        }
    }
}
