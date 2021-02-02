using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adnc.Core.Shared.Entities;

namespace Adnc.Usr.Core.Entities
{
	/// <summary>
	/// 部门
	/// </summary>
	[Table("SysDept")]
	[Description("部门")]
	public class SysDept : EfFullAuditEntity
	{
		//private ICollection<SysUser> _users;
		//private Action<object, string> LazyLoader { get; set; }
		//private SysDept(Action<object, string> lazyLoader)        
		//{
		//	LazyLoader = lazyLoader;
		//}

		public SysDept()
        {
        }

		[StringLength(32)]
		[Column("FullName")]
		public string FullName { get; set; }

		[Column("Num")]
		public int? Num { get; set; }

		[Column("Pid")]
		public long? Pid { get; set; }

		[StringLength(80)]
		[Column("Pids")]
		public string Pids { get; set; }

		[StringLength(16)]
		[Column("SimpleName")]
		public string SimpleName { get; set; }

		[StringLength(64)]
		[Column("Tips")]
		public string Tips { get; set; }

		[Column("Version")]
		public int? Version { get; set; }

		public virtual ICollection<SysUser> Users
		{
			//get => LazyLoader.Load(this, ref _users);
			//set => _users = value;
			get;
			set;
		}
	}
}
