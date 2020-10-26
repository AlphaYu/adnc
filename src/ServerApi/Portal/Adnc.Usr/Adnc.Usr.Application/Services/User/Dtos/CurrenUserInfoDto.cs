using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    public class CurrenUserInfoDto
    {
        public long ID { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }

        public string RemoteIpAddress { get; set; }

        public string Device { get; set; } = "web";
    }
}
