using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Entities
{
	/// <summary>
	/// 登录日志
	/// </summary>
	[Table("SysLoginLog")]
	[Description("登录日志")]
	public class SysLoginLog
	{
		[Key]
		[Column("ID")]
		public long ID { get; set; }

		[StringLength(20)]
		[Column("Device")]
		public string Device { get; set; }

		[StringLength(255)]
		[Column("Message")]
		public string Message { get; set; }

		[Column("Succeed")]
		public bool Succeed { get; set; }

		[Column("UserId")]
		public long? UserId { get; set; }

		[StringLength(32)]
		[Column("Account")]
		public string Account { get; set; }

        [StringLength(64)]
        [Column("UserName")]

        public string UserName { get; set; }

        [Column("RemoteIpAddress")]
        [StringLength(22)]
        public string RemoteIpAddress { get; set; }

		[Description("创建时间")]
		[Column("CreateTime")]
		public DateTime? CreateTime { get; set; }
	}
}
