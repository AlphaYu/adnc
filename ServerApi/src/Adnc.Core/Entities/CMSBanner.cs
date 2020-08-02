using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 文章
	/// </summary>
	[Table("CMSBanner")]
	[Description("文章")]
	public class CMSBanner
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

		[Key]
		[Column("ID")]
		public long ID { get; set; }

		/// <summary>
		/// banner图id
		/// </summary>
		[Description("banner图id")]
		[Column("IdFile")]
		public long? IdFile { get; set; }

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
		/// 类型
		/// </summary>
		[Description("类型")]
		[StringLength(32)]
		[Column("Type")]
		public string Type { get; set; }

		/// <summary>
		/// 点击banner跳转到url
		/// </summary>
		[Description("点击banner跳转到url")]
		[StringLength(128)]
		[Column("Url")]
		public string Url { get; set; }
	}
}
