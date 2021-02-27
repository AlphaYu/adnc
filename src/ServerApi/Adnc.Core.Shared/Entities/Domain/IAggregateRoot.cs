using Adnc.Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Shared.Domain.Entities
{
    public interface IAggregateRoot : IEfEntity<long>
    {
        public DateTime? RowVersion { get; set; }
    }
}
