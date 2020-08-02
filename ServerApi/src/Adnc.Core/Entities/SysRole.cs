using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 角色
	/// </summary>
	[Table("SysRole")]
	[Description("角色")]
	public class SysRole : EfEntity<long>
	{
		[Column("DeptId")]
		public long? DeptId { get; set; }

		[StringLength(255)]
		[Column("Name")]
		public string Name { get; set; }

		[Column("Num")]
		public int? Num { get; set; }

		[Column("Pid")]
		public long? PID { get; set; }

		[StringLength(255)]
		[Column("Tips")]
		public string Tips { get; set; }

		[Column("Version")]
		public int? Version { get; set; }

		public virtual Collection<SysRelation> Relations { get; set; }
	}
}
