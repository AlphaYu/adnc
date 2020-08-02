using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 历史消息
	/// </summary>
	[Table("Message")]
	[Description("历史消息")]
	public class Message
	{
		/// <summary>
		/// 消息内容
		/// </summary>
		[Description("消息内容")]
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
		/// 接收者
		/// </summary>
		[Description("接收者")]
		[StringLength(64)]
		[Column("Receiver")]
		public string Receiver { get; set; }

		/// <summary>
		/// 消息类型,0:初始,1:成功,2:失败
		/// </summary>
		[Description("消息类型,0:初始,1:成功,2:失败")]
		[StringLength(32)]
		[Column("State")]
		public string State { get; set; }

		/// <summary>
		/// 模板编码
		/// </summary>
		[Description("模板编码")]
		[StringLength(32)]
		[Column("TplCode")]
		public string TplCode { get; set; }

		/// <summary>
		/// 消息类型,0:短信,1:邮件
		/// </summary>
		[Description("消息类型,0:短信,1:邮件")]
		[StringLength(32)]
		[Column("Type")]
		public string Type { get; set; }
	}
}
