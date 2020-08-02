using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 消息发送者
	/// </summary>
	[Table("MessageSender")]
	[Description("消息发送者")]
	public class MessageSender
	{
		/// <summary>
		/// 发送类
		/// </summary>
		[Description("发送类")]
		[StringLength(64)]
		[Column("ClassName")]
		public string ClassName { get; set; }

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

		[Key]
		[Column("ID")]
		public long ID { get; set; }

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
		/// 名称
		/// </summary>
		[Description("名称")]
		[StringLength(64)]
		[Column("Name")]
		public string Name { get; set; }

		/// <summary>
		/// 短信运营商模板编号
		/// </summary>
		[Description("短信运营商模板编号")]
		[StringLength(64)]
		[Column("TplCode")]
		public string TplCode { get; set; }
	}
}
