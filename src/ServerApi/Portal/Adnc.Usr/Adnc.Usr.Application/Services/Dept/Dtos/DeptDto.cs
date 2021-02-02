using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
	/// <summary>
	/// 部门
	/// </summary>
	[Serializable]
	public class DeptDto : BaseOutputDto
	{
		/// <summary>
		/// 部门全称
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		public int? Num { get; set; }

		/// <summary>
		/// 父级Id
		/// </summary>
		public long? Pid { get; set; }

		/// <summary>
		/// 父级Id集合
		/// </summary>
		public string Pids { get; set; }

		/// <summary>
		/// 部门简称
		/// </summary>
		public string SimpleName { get; set; }

		/// <summary>
		/// 部门描述
		/// </summary>
		public string Tips { get; set; }

		/// <summary>
		/// 版本号
		/// </summary>
		public int? Version { get; set; }
	}
}
