using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 文章
	/// </summary>
	[Table("CMSArticle")]
	[Description("文章")]
	public class CMSArticle
	{
		/// <summary>
		/// 作者
		/// </summary>
		[Description("作者")]
		[StringLength(64)]
		[Column("Author")]
		public string Author { get; set; }

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
		/// 栏目id
		/// </summary>
		[Description("栏目id")]
		[Column("IdChannel")]
		public long IdChannel { get; set; }

		/// <summary>
		/// 文章题图ID
		/// </summary>
		[Description("文章题图ID")]
		[StringLength(64)]
		[Column("Img")]
		public string Img { get; set; }

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
		[StringLength(128)]
		[Column("Title")]
		public string Title { get; set; }
	}
}
