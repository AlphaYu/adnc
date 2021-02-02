using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adnc.Core.Shared.Entities;

namespace Adnc.Usr.Core.Entities
{
	/// <summary>
	/// 角色
	/// </summary>
	[Table("SysRole")]
	[Description("角色")]
	public class SysRole : EfFullAuditEntity
	{
		[Column("DeptId")]
		public long? DeptId { get; set; }

		[StringLength(32)]
		[Column("Name")]
		public string Name { get; set; }

		[Column("Num")]
		public int? Num { get; set; }

		[Column("Pid")]
		public long? PID { get; set; }

		[StringLength(64)]
		[Column("Tips")]
		public string Tips { get; set; }

		[Column("Version")]
		public int? Version { get; set; }

		public virtual Collection<SysRelation> Relations { get; set; }
	}
}
