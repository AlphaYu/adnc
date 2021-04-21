using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace  Adnc.Maint.Application.Contracts.Dtos
{
	/// <summary>
	/// 系统通知
	/// </summary>
	public class NoticeDto : OutputBaseAuditDto<long>
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
