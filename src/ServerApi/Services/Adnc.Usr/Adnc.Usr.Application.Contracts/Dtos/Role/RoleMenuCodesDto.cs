using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Contracts.Dtos
{
	/// <summary>
	/// 角色，权限
	/// </summary>
	[Serializable]
    public class RoleMenuCodesDto : IDto
    {
        /// <summary>
        /// 菜单Code
        /// </summary>
        public string Code { get; set; }

		/// <summary>
		/// 角色Id
		/// </summary>
		public long RoleId { get; set; }
	}
}
