using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adnc.Core.Shared.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Adnc.Maint.Core.Entities
{
	/// <summary>
	/// 登录日志
	/// </summary>
	//[Table("SysLoginLog")]
	//[Description("登录日志")]
    public class SysLoginLog : MongoEntity
	{

        [StringLength(16)]
		//[Column("Device")]
		public string Device { get; set; }

		[StringLength(255)]
		//[Column("Message")]
		public string Message { get; set; }

		//[Column("Succeed")]
		public bool Succeed { get; set; }

		public int StatusCode { get; set; }

		//[Column("UserId")]
		public long? UserId { get; set; }

		//[StringLength(16)]
		[Column("Account")]
		public string Account { get; set; }

        //[StringLength(16)]
        [Column("UserName")]

        public string UserName { get; set; }

        //[Column("RemoteIpAddress")]
        [StringLength(15)]
        public string RemoteIpAddress { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime? CreateTime { get; set; }
	}
}
