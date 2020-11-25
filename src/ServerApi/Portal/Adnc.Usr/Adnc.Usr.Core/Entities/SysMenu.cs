using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adnc.Core.Shared.Entities;

namespace Adnc.Usr.Core.Entities
{
	/// <summary>
	/// 菜单
	/// </summary>
	[Table("SysMenu")]
	[Description("菜单")]
	public class SysMenu : EfAuditEntity
	{
		/// <summary>
		/// 编号
		/// </summary>
		[Description("编号")]
		[StringLength(16)]
		[Column("Code")]
		public string Code { get; set; }

		/// <summary>
		/// 組件配置
		/// </summary>
		[Description("組件配置")]
		[StringLength(64)]
		[Column("Component")]
		public string Component { get; set; }

		/// <summary>
		/// 是否隐藏
		/// </summary>
		[Description("是否隐藏")]
		[Column("Hidden")]
		public bool? Hidden { get; set; }

		/// <summary>
		/// 图标
		/// </summary>
		[Description("图标")]
		[StringLength(16)]
		[Column("Icon")]
		public string Icon { get; set; }

		/// <summary>
		/// 是否是菜单1:菜单,0:按钮
		/// </summary>
		[Description("是否是菜单1:菜单,0:按钮")]
		[Column("IsMenu")]
		public bool IsMenu { get; set; }

		/// <summary>
		/// 是否默认打开1:是,0:否
		/// </summary>
		[Description("是否默认打开1:是,0:否")]
		[Column("IsOpen")]
		public bool? IsOpen { get; set; }

		/// <summary>
		/// 级别
		/// </summary>
		[Description("级别")]
		[Column("Levels")]
		public int Levels { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[Description("名称")]
		[StringLength(16)]
		[Column("Name")]
		public string Name { get; set; }

		/// <summary>
		/// 顺序
		/// </summary>
		[Description("顺序")]
		[Column("Num")]
		public int Num { get; set; }

		/// <summary>
		/// 父菜单编号
		/// </summary>
		[Description("父菜单编号")]
		[StringLength(16)]
		[Column("PCode")]
		public string PCode { get; set; }

		/// <summary>
		/// 递归父级菜单编号
		/// </summary>
		[Description("递归父级菜单编号")]
		[StringLength(128)]
		[Column("PCodes")]
		public string PCodes { get; set; }

		/// <summary>
		/// 状态1:启用,0:禁用
		/// </summary>
		[Description("状态1:启用,0:禁用")]
		[Column("Status")]
		public bool Status { get; set; }

		/// <summary>
		/// 鼠标悬停提示信息
		/// </summary>
		[Description("鼠标悬停提示信息")]
		[StringLength(32)]
		[Column("Tips")]
		public string Tips { get; set; }

		/// <summary>
		/// 链接
		/// </summary>
		[Description("链接")]
		[StringLength(64)]
		[Column("Url")]
		public string Url { get; set; }

		public virtual Collection<SysRelation> Relations { get; set; }
	}
}
