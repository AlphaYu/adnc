using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 菜单角色关系
	/// </summary>
	[Table("SysRelation")]
	[Description("菜单角色关系")]
    public class SysRelation : EfEntity
    {
        [Column("MenuId")]
		public long MenuId { get; set; }

		[Column("RoleId")]
		public long RoleId { get; set; }

		public virtual SysRole Role { get; set; }

		public virtual SysMenu Menu { get; set; }
	}
}
