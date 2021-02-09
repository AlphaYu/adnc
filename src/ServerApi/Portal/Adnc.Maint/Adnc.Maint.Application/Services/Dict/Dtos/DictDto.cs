using Adnc.Application.Shared.Dtos;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Adnc.Maint.Application.Dtos
{
    [Serializable]
    public class DictDto : OutputDto<long>
    {
        public string Name { get; set; }

        public int Ordinal { get; set; }

        public long? Pid { get; set; }

        public string Value { get; set; }

        private IList<DictDto> _data = Array.Empty<DictDto>();
        [NotNull]
        public IList<DictDto> Children
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
