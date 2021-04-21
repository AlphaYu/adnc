using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace  Adnc.Maint.Application.Dtos
{
	public class TaskLogDto:BaseOutputDto
	{
		/// <summary>
		/// 执行时间
		/// </summary>
		public DateTime? ExecAt { get; set; }

		/// <summary>
		/// 执行结果（成功:1、失败:0)
		/// </summary>
		public bool? ExecSuccess { get; set; }


		public long? IdTask { get; set; }

		/// <summary>
		/// 抛出异常
		/// </summary>
		public string JobException { get; set; }

		/// <summary>
		/// 任务名
		/// </summary>
		public string Name { get; set; }
	}
}
