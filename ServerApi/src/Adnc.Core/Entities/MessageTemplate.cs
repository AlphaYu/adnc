using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 消息模板
	/// </summary>
	[Table("MessageTemplate")]
	[Description("消息模板")]
	public class MessageTemplate
	{
		/// <summary>
		/// 编号
		/// </summary>
		[Description("编号")]
		[StringLength(32)]
		[Column("Code")]
		public string Code { get; set; }

		/// <summary>
		/// 发送条件
		/// </summary>
		[Description("发送条件")]
		[StringLength(32)]
		[Column("Cond")]
		public string Cond { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		[Description("内容")]
		[StringLength(65535)]
		[Column("Content")]
		public string Content { get; set; }

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
		/// 发送者id
		/// </summary>
		[Description("发送者id")]
		[Key]
		[Column("IDMessageSender")]
		public long IDMessageSender { get; set; }

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
		/// 标题
		/// </summary>
		[Description("标题")]
		[StringLength(64)]
		[Column("Title")]
		public string Title { get; set; }

		/// <summary>
		/// 消息类型,0:短信,1:邮件
		/// </summary>
		[Description("消息类型,0:短信,1:邮件")]
		[StringLength(32)]
		[Column("Type")]
		public string Type { get; set; }
	}
}
