using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adnc.Core.Shared.Entities;

namespace Adnc.Maint.Core.Entities
{
	/// <summary>
	/// 定时任务日志
	/// </summary>
	[Table("SysTaskLog")]
	[Description("定时任务日志")]
    public class SysTaskLog : EfEntity
    {
        /// <summary>
        /// 执行时间
        /// </summary>
        [Description("执行时间")]
		[Column("ExecAt")]
		public DateTime? ExecAt { get; set; }

		/// <summary>
		/// 执行结果（成功:1、失败:0)
		/// </summary>
		[Description("执行结果（成功:1、失败:0)")]
		[Column("ExecSuccess")]
		public bool? ExecSuccess { get; set; }

		[Column("IdTask")]
		public long? IdTask { get; set; }

		/// <summary>
		/// 抛出异常
		/// </summary>
		[Description("抛出异常")]
		[StringLength(500)]
		[Column("JobException")]
		public string JobException { get; set; }

		/// <summary>
		/// 任务名
		/// </summary>
		[Description("任务名")]
		[StringLength(50)]
		[Column("Name")]
		public string Name { get; set; }
	}
}
