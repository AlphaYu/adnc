using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Adnc.Shared.RpcServices.Rtos
{
    public class DictRto
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string Num { get; set; }

        public long? Pid { get; set; }

        public string Value { get; set; }

        private IReadOnlyList<DictRto> _data = Array.Empty<DictRto>();

        [NotNull]
        public IReadOnlyList<DictRto> Children
        {
            get => _data;
            set
            {
                if (value != null)
                {
                    _data = value;
                }
            }
        }
    }
}