using Adnc.Core.Shared.Domain.Entities;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Orders.Domain.Entities
{
    public class OrderItemProduct : ValueObject
    {
        public long Id { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        private OrderItemProduct() { }

        public OrderItemProduct(long id, string name, decimal price)
        {
            this.Id = Checker.GTZero(id, nameof(id));
            this.Name = Checker.NotNullOrEmpty(name, nameof(name));
            this.Price = Checker.GTZero(price, nameof(price));
        }
    }
}
