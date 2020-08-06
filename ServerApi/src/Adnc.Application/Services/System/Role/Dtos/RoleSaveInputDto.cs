using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    public class RoleSaveInputDto : BaseInputDto
    {
		/// <summary>
		/// 部门ID
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
		/// 父级角色ID
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
