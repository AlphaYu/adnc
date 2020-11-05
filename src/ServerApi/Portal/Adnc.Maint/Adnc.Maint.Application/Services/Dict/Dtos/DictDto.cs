using Adnc.Application.Shared.Dtos;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace  Adnc.Maint.Application.Dtos
{
	[Serializable]
	public class DictDto : BaseOutputDto
	{
		public string Name { get; set; }

		public string Num { get; set; }

		public long? Pid { get; set; }

		public string Tips { get; set; }

		public string Detail { get; set; }

        /// <summary>
        /// 该属性可以和Detail重复了。
        /// 考虑到合并需要修改前端代码，先这样。
        /// </summary>
        private IReadOnlyList<DictDto> _data = Array.Empty<DictDto>();
        [NotNull]
        public IReadOnlyList<DictDto> Children
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
