using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace  Adnc.Maint.Application.Dtos
{
	public class TaskDto : BaseOutputDto
	{
		/// <summary>
		/// 是否允许并发
		/// </summary>
		public bool Concurrent { get; set; }

		/// <summary>
		/// 定时规则
		/// </summary>
		public string Cron { get; set; }

		/// <summary>
		/// 执行参数
		/// </summary>
		public string Data { get; set; }

		/// <summary>
		/// 是否禁用
		/// </summary>
		public bool Disabled { get; set; }

		/// <summary>
		/// 执行时间
		/// </summary>
		public DateTime? ExecAt { get; set; }

		/// <summary>
		/// 执行结果
		/// </summary>
		public string ExecResult { get; set; }

		/// <summary>
		/// 执行类
		/// </summary>
		public string JobClass { get; set; }

		/// <summary>
		/// 任务组名
		/// </summary>
		public string JobGroup { get; set; }

		/// <summary>
		/// 任务名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 任务说明
		/// </summary>
		public string Note { get; set; }
	}
}
