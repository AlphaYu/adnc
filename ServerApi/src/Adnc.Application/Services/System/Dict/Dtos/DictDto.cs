using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
	[Serializable]
	public class DictDto : BaseOutputDto
	{
		public string Name { get; set; }

		public string Num { get; set; }

		public long? Pid { get; set; }

		public string Tips { get; set; }

		public string Detail { get; set; }
	}
}
