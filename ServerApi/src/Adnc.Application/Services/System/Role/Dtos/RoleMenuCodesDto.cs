using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
	/// <summary>
	/// 角色，权限
	/// </summary>
	[Serializable]
	public class RoleMenuCodesDto
	{
		/// <summary>
		/// 菜单Code
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 角色ID
		/// </summary>
		public long RoleId { get; set; }
	}
}
