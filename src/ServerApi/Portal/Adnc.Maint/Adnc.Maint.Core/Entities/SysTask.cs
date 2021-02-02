using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adnc.Core.Shared.Entities;

namespace Adnc.Maint.Core.Entities
{
	/// <summary>
	/// 定时任务
	/// </summary>
	[Table("SysTask")]
	[Description("定时任务")]
	public class SysTask : EfFullAuditEntity
	{
		/// <summary>
		/// 是否允许并发
		/// </summary>
		[Description("是否允许并发")]
		[Column("Concurrent")]
		public bool? Concurrent { get; set; }

		/// <summary>
		/// 定时规则
		/// </summary>
		[Description("定时规则")]
		[StringLength(50)]
		[Column("Cron")]
		public string Cron { get; set; }

		/// <summary>
		/// 执行参数
		/// </summary>
		[Description("执行参数")]
		[StringLength(65535)]
		[Column("Data")]
		public string Data { get; set; }

		/// <summary>
		/// 是否禁用
		/// </summary>
		[Description("是否禁用")]
		[Column("Disabled")]
		public bool? Disabled { get; set; }

		/// <summary>
		/// 执行时间
		/// </summary>
		[Description("执行时间")]
		[Column("ExecAt")]
		public DateTime? ExecAt { get; set; }

		/// <summary>
		/// 执行结果
		/// </summary>
		[Description("执行结果")]
		[StringLength(65535)]
		[Column("ExecResult")]
		public string ExecResult { get; set; }

		/// <summary>
		/// 执行类
		/// </summary>
		[Description("执行类")]
		[StringLength(255)]
		[Column("JobClass")]
		public string JobClass { get; set; }

		/// <summary>
		/// 任务组名
		/// </summary>
		[Description("任务组名")]
		[StringLength(50)]
		[Column("JobGroup")]
		public string JobGroup { get; set; }

		/// <summary>
		/// 任务名
		/// </summary>
		[Description("任务名")]
		[StringLength(50)]
		[Column("Name")]
		public string Name { get; set; }

		/// <summary>
		/// 任务说明
		/// </summary>
		[Description("任务说明")]
		[StringLength(255)]
		[Column("Note")]
		public string Note { get; set; }
	}
}
