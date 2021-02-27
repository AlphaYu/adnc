using Adnc.Core.Shared.Domain.Entities;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Ord.Domain.Entities
{
    public class OrderItemProduct : ValueObject
    {
        public long Id { get;}

        public string Name { get; }

        public decimal Price { get; }

        private OrderItemProduct() { }

        public OrderItemProduct(long id, string name, decimal price)
        {
            this.Id = Checker.GTZero(id, nameof(id));
            this.Name = Checker.NotNullOrEmpty(name, nameof(name));
            this.Price = Checker.GTZero(price, nameof(price));
        }
    }
}
