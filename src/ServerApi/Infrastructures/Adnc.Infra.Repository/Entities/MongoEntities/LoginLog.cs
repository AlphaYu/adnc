using MongoDB.Bson.Serialization.Attributes;

namespace Adnc.Infra.Entities
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class LoginLog : MongoEntity
    {
        public string Device { get; set; }

        public string Message { get; set; }

        public bool Succeed { get; set; }

        public int StatusCode { get; set; }

        public long? UserId { get; set; }

        public string Account { get; set; }

        public string UserName { get; set; }

        public string RemoteIpAddress { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreateTime { get; set; }
    }
}