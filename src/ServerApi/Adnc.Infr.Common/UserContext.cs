using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Infr.Common
{
    /// <summary>
    /// 当前用户上下文,放adnc.infr.common这里欠妥。
    /// </summary>
    public class UserContext
    {
        public long ID { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }

        public string RemoteIpAddress { get; set; }

        public string Device { get; set; }

        public string Email { get; set; }

        public long[] RoleIds { get; set; }
    }
}
