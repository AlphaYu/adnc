using System;

namespace Adnc.Ord.Domain
{
    public interface IDomainEvent
    {
        public DateTime OccurredOn { get; }
    }
}
