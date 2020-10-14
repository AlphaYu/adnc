using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Infr.Mq.RabbitMq
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}
