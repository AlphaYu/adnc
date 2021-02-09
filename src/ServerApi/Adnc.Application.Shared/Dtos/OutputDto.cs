using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Shared.Dtos
{
    [Serializable]
    public abstract class OutputDto<TKey> : IOutputDto<TKey>
    {
        public TKey Id { get; set; }
    }
}
