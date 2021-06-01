using System.Collections.Generic;

namespace Adnc.Whse.Application.EventSubscribers.Etos
{
    public class OrderCreatedEventData
    {
        public long OrderId { get; set; }

        public ICollection<(long ProductId, int Qty)> Products { get; set; }
    }
}