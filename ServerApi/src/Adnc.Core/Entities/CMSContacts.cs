using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 邀约信息
	/// </summary>
	[Table("CMSContacts")]
	[Description("邀约信息")]
	public class CMSContacts
	{
		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
		[Column("CreateBy")]
		public long? CreateBy { get; set; }

		/// <summary>
		/// 创建时间/注册时间
		/// </summary>
		[Description("创建时间/注册时间")]
		[Column("CreateTime")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 电子邮箱
		/// </summary>
		[Description("电子邮箱")]
		[StringLength(32)]
		[Column("Email")]
		public string Email { get; set; }

		[Key]
		[Column("ID")]
		public long ID { get; set; }

		/// <summary>
		/// 联系电话
		/// </summary>
		[Description("联系电话")]
		[StringLength(64)]
		[Column("Mobile")]
		public string Mobile { get; set; }

		/// <summary>
		/// 最后更新人
		/// </summary>
		[Description("最后更新人")]
		[Column("ModifyBy")]
		public long? ModifyBy { get; set; }

		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		[Column("ModifyTime")]
		public DateTime? ModifyTime { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		[StringLength(128)]
		[Column("Remark")]
		public string Remark { get; set; }

		/// <summary>
		/// 邀约人名称
		/// </summary>
		[Description("邀约人名称")]
		[StringLength(64)]
		[Column("UserName")]
		public string UserName { get; set; }
	}
}
