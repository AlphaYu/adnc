using Adnc.Core.Shared.Events;
using System.Collections.Generic;

namespace Adnc.Orders.Domain.Events.Etos
{
    public class OrderCreatedEventEto : BaseEto
    {
        public long OrderId { get; set; }
        public ICollection<Product> Products { get; set; }

        public class Product
        {
            public long ProductId { get; set; }

            public int Qty { get; set; }
        }
    }
}
