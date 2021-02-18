using Adnc.Core.Shared.Domain.Entities;

namespace Adnc.Orders.Domain.Entities
{
    public class OrderDeliveryInfomation : ValueObject
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
