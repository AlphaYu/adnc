using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Entities
{
    public interface IEfEntity<T>
    {
        public T ID { get; set; }
    }
}
