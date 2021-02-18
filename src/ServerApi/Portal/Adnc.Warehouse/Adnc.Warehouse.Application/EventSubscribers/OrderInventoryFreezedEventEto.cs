using Adnc.Core.Shared.Events;

namespace Adnc.Warehouse.Application
{
    public class OrderInventoryFreezedEventEto : BaseEto
    {
        public long OrderId { get; set; }

        public bool IsSuccess { get; set; }
    }
}
