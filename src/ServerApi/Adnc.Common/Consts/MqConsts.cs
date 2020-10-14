using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Common.Consts
{
    public static class MqConsts
    {
        public static class Exchanges
        {
            public const string Logs = "ex-adnc-logs";
            public const string Dead = "ex-adnc-dead-letter";
        }

        public static class RoutingKeys
        {
            public const string Loginlog = "loginlog";
            public const string OpsLog = "opslog";
            public const string Sms = "sms";
            public const string Email = "email";
        }
    }
}
