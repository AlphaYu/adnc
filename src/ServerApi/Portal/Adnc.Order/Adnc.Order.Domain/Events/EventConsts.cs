using Adnc.Core.Shared;

namespace Adnc.Orders.Domain.Events
{
    public sealed class EventConsts: BaseEbConsts
    {
        public const string OrderCreatedEvent = "OrderCreatedEvent";
        public const string OrderInventoryFreezedEvent = "OrderInventoryFreezedEvent";
    }
}
