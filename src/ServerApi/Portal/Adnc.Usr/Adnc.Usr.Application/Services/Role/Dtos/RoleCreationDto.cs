﻿using Adnc.Application.Shared.Dtos;

namespace Adnc.Usr.Application.Dtos
{
    public class RoleCreationDto : IInputDto
    {
		/// <summary>
		/// 角色名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 角色描述
		/// </summary>
		public string Tips { get; set; }

		/// <summary>
		/// 序号
		/// </summary>
		public int Ordinal { get; set; }

	}
}
