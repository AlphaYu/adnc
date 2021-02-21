using Adnc.Core.Shared.Domain.Entities;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Orders.Domain.Entities
{
    public class OrderDeliveryInfomation : ValueObject
    {
        public string Name { get; }
        public string Phone { get; }
        public string Address { get; }

        public OrderDeliveryInfomation(string name, string phone, string address)
        {
            this.Name = Checker.NotNullOrEmpty(name, nameof(name));
            this.Phone = Checker.NotNullOrEmpty(phone, nameof(phone));
            this.Address = Checker.NotNullOrEmpty(address, nameof(address));
        }
    }
}
