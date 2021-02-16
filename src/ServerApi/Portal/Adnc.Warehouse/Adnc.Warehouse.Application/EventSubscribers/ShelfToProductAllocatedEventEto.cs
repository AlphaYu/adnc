using Adnc.Core.Shared.Events;

namespace Adnc.Warehouse.Application.EventSubscribers
{
    public class ShelfToProductAllocatedEventEto : BaseEto
    {
        public long ShelfId { get; set; }

        public long ProductId { get; set; }
    }
}
