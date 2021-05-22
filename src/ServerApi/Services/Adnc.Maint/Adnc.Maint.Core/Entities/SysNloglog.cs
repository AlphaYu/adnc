using Adnc.Core.Shared.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Adnc.Core.Maint.Entities
{
    /// <summary>
    /// 异常日志
    /// </summary>
    //the driver would ignore any extra fields instead of throwing an exception
    [BsonIgnoreExtraElements]
    public class SysNloglog : MongoEntity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Logger { get; set; }

        public string Exception { get; set; }

        public int ThreadID { get; set; }

        public string ThreadName { get; set; }

        public int ProcessID { get; set; }

        public string ProcessName { get; set; }

        public SysNloglogProperty Properties { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SysNloglogProperty
    {
        public string TraceIdentifier { get; set; }

        public string ConnectionId { get; set; }

        public string EventId_Id { get; set; }

        public string EventId_Name { get; set; }

        public string EventId { get; set; }

        public string RemoteIpAddress { get; set; }

        public string BaseDir { get; set; }

        public string QueryUrl { get; set; }

        public string RequestMethod { get; set; }

        public string Controller { get; set; }

        public string Method { get; set; }

        public string FormContent { get; set; }

        public string QueryContent { get; set; }
    }
}