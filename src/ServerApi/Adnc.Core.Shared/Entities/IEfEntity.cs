using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Shared.Entities
{
    public interface IEfEntity<T>
    {
        public T ID { get; set; }
    }
}
