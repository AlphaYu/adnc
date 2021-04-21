using System;
using MongoDB.Bson.Serialization.Attributes;
using Adnc.Core.Shared.Entities;

namespace Adnc.Core.Maint.Entities
{
	/// <summary>
	/// 操作日志
	/// </summary>
    public class SysOperationLog : MongoEntity
    {
		public string ClassName { get; set; }

		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime? CreateTime { get; set; }

		public string LogName { get; set; }

		public string LogType { get; set; }

		public string Message { get; set; }

		public string Method { get; set; }

		public string Succeed { get; set; }

		public long? UserId { get; set; }

		public string Account { get; set; }

		public string UserName { get; set; }

		public string RemoteIpAddress { get; set; }
	}
}
