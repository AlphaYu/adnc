using System;
using Adnc.Core.Shared.Entities;

namespace Adnc.Core.Shared.Domain.Entities
{
    public abstract class AggregateRoot : Entity, IAggregateRoot<long>, IConcurrency
    {
        /// <summary>
        /// 并发控制列
        /// </summary>
        public DateTime? RowVersion { get; set; }
    }
}