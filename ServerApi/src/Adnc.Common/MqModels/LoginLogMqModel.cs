using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Common.MqModels
{
    public class LoginLogMqModel
    {
        public long ID { get; set; }

        public string Device { get; set; }

        public string Message { get; set; }

        public bool Succeed { get; set; }

        public long? UserId { get; set; }

        public string Account { get; set; }

        public string UserName { get; set; }

        public string RemoteIpAddress { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
