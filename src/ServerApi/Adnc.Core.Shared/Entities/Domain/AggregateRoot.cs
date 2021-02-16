using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Core.Shared.Entities;

namespace Adnc.Core.Shared.Domain.Entities
{
    public abstract class AggregateRoot : EfEntity, IAggregateRoot
    {
    }
}
