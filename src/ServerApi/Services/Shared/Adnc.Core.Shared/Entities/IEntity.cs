using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Shared.Entities
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
