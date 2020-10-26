using System;

namespace Adnc.Maint.Application.Mq
{
    public class OpsLogMqModel
    {
        public string Id { get; set; }

        public string ClassName { get; set; }

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
