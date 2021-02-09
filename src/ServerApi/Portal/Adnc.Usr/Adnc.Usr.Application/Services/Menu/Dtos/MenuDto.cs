﻿using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
	/// <summary>
	/// 菜单
	/// </summary>
	[Serializable]
	public class MenuDto : OutputDto<long>
	{
		/// <summary>
		/// 编号
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 組件配置
		/// </summary>
		public string Component { get; set; }

		/// <summary>
		/// 是否隐藏
		/// </summary>
		public bool? Hidden { get; set; }

		/// <summary>
		/// 图标
		/// </summary>
		public string Icon { get; set; }

		/// <summary>
		/// 是否是菜单1:菜单,0:按钮
		/// </summary>
		public bool IsMenu { get; set; }

		/// <summary>
		/// 是否默认打开1:是,0:否
		/// </summary>
		public bool? IsOpen { get; set; }

		/// <summary>
		/// 级别
		/// </summary>
		public int Levels { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 序号
		/// </summary>
		public int Ordinal { get; set; }

		/// <summary>
		/// 父菜单编号
		/// </summary>
		public string PCode { get; set; }

		/// <summary>
		/// 递归父级菜单编号
		/// </summary>
		public string PCodes { get; set; }

		/// <summary>
		/// 状态1:启用,0:禁用
		/// </summary>
		public bool Status { get; set; }

		/// <summary>
		/// 鼠标悬停提示信息
		/// </summary>
		public string Tips { get; set; }

		/// <summary>
		/// 链接
		/// </summary>
		public string Url { get; set; }
	}
}
