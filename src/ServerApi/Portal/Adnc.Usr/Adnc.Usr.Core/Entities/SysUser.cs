using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adnc.Core.Shared.Entities;

namespace Adnc.Usr.Core.Entities
{
	/// <summary>
	/// 账号
	/// </summary>
	[Table("SysUser")]
	[Description("账号")]
	public class SysUser : EfFullAuditEntity,ISoftDelete
	{

		//private SysDept _dept;
		//private Action<object, string> LazyLoader { get; set; }
		//private SysUser(Action<object, string> lazyLoader)
		//{
		//	LazyLoader = lazyLoader;
		//}
        public SysUser()
        {
        }

		/// <summary>
		/// 账户
		/// </summary>
		[Description("账户")]
		[StringLength(16)]
		[Column("Account")]
		public string Account { get; set; }

		[StringLength(64)]
		[Column("Avatar")]
		public string Avatar { get; set; }

		[Column("Birthday")]
		public DateTime? Birthday { get; set; }

		[Column("DeptId")]
		public long? DeptId { get; set; }

		public virtual SysDept Dept
		{
			//get => LazyLoader.Load(this, ref _dept);
			//set => _dept = value;
			get;
			set;
		}

        /// <summary>
        /// email
        /// </summary>
        [Description("email")]
		[StringLength(32)]
		[Column("Email")]
		public string Email { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
		[Description("姓名")]
		[StringLength(16)]
		[Column("Name")]
		public string Name { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Description("密码")]
		[StringLength(32)]
		[Column("Password")]
		public string Password { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		[Description("手机号")]
		[StringLength(11)]
		[Column("Phone")]
		public string Phone { get; set; }

		/// <summary>
		/// 角色id列表，以逗号分隔
		/// </summary>
		[Description("角色id列表，以逗号分隔")]
		[StringLength(72)]
		[Column("RoleIds")]
		public string RoleIds { get; set; }

		/// <summary>
		/// 密码盐
		/// </summary>
		[Description("密码盐")]
		[StringLength(6)]
		[Column("Salt")]
		public string Salt { get; set; }

		[Column("Sex")]
		public int Sex { get; set; }

		[Column("Status")]
		public int Status { get; set; }

		[Column("Version")]
		public int? Version { get; set; }

        public bool IsDeleted { get; set; }

		public virtual SysUserFinance UserFinance { get; set; }
	}
}
	