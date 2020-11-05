using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Cus.Application.RpcServices
{
    public class GetDictReply
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string Num { get; set; }

        public long? Pid { get; set; }

        public string Tips { get; set; }

        private IReadOnlyList<GetDictReply> _data = Array.Empty<GetDictReply>();
        [NotNull]
        public IReadOnlyList<GetDictReply> Children
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
