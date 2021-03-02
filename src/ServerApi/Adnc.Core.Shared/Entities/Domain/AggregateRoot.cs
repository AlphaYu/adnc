using Adnc.Core.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace Adnc.Core.Shared.Domain.Entities
{
    public abstract class AggregateRoot : Entity, IAggregateRoot<long>, IConcurrency
    {
        public byte[] RowVersion { get; set; }
    }
}