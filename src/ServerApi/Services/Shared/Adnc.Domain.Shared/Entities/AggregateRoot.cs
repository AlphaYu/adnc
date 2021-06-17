using Adnc.Infra.Entities;

namespace Adnc.Domain.Shared.Entities
{
    public abstract class AggregateRoot : DomainEntity, IConcurrency, IEfEntity<long>
    {
        public byte[] RowVersion { get; set; }
    }
}