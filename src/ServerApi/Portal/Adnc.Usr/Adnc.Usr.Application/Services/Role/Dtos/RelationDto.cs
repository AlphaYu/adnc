using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    /// <summary>
    /// 菜单-角色关联
    /// </summary>
	[Serializable]
    public class RelationDto : BaseDto
    {
		/// <summary>
		/// 菜单Id
		/// </summary>
		public long? MenuId { get; set; }

		/// <summary>
		/// 角色Id
		/// </summary>
		public long? RoleId { get; set; }
	}
}
