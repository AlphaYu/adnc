using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
	/// <summary>
	/// 系统通知
	/// </summary>
	public class NoticeDto : BaseDto<long>
	{
		/// <summary>
		/// 内容
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 类型
		/// </summary>
		public int? Type { get; set; }
	}
}
