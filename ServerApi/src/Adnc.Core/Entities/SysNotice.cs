using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 通知
	/// </summary>
	[Table("SysNotice")]
	[Description("通知")]
	public class SysNotice : EfAuditEntity
	{
		[StringLength(255)]
		[Column("Content")]
		public string Content { get; set; }

		[StringLength(255)]
		[Column("Title")]
		public string Title { get; set; }

		[Column("Type")]
		public int? Type { get; set; }
	}
}
