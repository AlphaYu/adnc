using Adnc.Core.Shared.Domain.Entities;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Ord.Domain.Entities
{
    public class OrderReceiver : ValueObject
    {
        public string Name { get; }
        public string Phone { get; }
        public string Address { get; }

        public OrderReceiver(string name, string phone, string address)
        {
            this.Name = Checker.NotNullOrEmpty(name, nameof(name));
            this.Phone = Checker.NotNullOrEmpty(phone, nameof(phone));
            this.Address = Checker.NotNullOrEmpty(address, nameof(address));
        }
    }
}
