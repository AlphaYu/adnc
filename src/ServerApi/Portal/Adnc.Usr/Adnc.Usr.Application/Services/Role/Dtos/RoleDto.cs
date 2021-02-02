using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
	/// <summary>
	/// 角色
	/// </summary>
	[Serializable]
	public class RoleDto :BaseOutputDto
	{
		/// <summary>
		/// 部门Id
		/// </summary>
		public long? DeptId { get; set; }

		/// <summary>
		/// 角色名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		public int? Num { get; set; }

		/// <summary>
		/// 父级角色Id
		/// </summary>
		public long? Pid { get; set; }

		/// <summary>
		/// 角色描述
		/// </summary>
		public string Tips { get; set; }

		/// <summary>
		/// 权限集合
		/// </summary>
		public string Permissions { get; set; }

		/// <summary>
		/// 角色版本号
		/// </summary>
		public int? Version { get; set; }
	}
}
