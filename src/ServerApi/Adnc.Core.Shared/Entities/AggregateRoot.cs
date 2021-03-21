using System;

namespace Adnc.Core.Shared.Entities
{
    public abstract class AggregateRoot : Entity, IAggregateRoot<long>, IConcurrency
    {
        public byte[] RowVersion { get; set; }
    }
}