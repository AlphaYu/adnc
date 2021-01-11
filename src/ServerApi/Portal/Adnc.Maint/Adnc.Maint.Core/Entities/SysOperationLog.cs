using Adnc.Core.Shared.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Adnc.Core.Maint.Entities
{
	/// <summary>
	/// 操作日志
	/// </summary>
	//[Table("SysOperationLog")]
	//[Description("操作日志")]
    public class SysOperationLog : MongoEntity
    {
		//[StringLength(255)]
		//[Column("ClassName")]
		public string ClassName { get; set; }

		//[Column("CreateTime")]
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime? CreateTime { get; set; }

		//[StringLength(255)]
		//[Column("LogName")]
		public string LogName { get; set; }

		//[StringLength(255)]
		//[Column("LogType")]
		public string LogType { get; set; }

		/// <summary>
		/// 详细信息
		/// </summary>
		//[Description("详细信息")]
		//[StringLength(65535)]
		//[Column("Message")]
		public string Message { get; set; }

		//[StringLength(255)]
		//[Column("Method")]
		public string Method { get; set; }

		//[StringLength(255)]
		//[Column("Succeed")]
		public string Succeed { get; set; }

		//[Column("UserId")]
		public long? UserId { get; set; }

		//[Column("Account")]
		[StringLength(32)]
		public string Account { get; set; }

		//[StringLength(64)]
		//[Column("UserName")]

		public string UserName { get; set; }

		//[Column("RemoteIpAddress")]
		//[StringLength(22)]
		public string RemoteIpAddress { get; set; }
	}
}
