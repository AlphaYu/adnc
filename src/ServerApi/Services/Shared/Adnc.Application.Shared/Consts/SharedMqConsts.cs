namespace Adnc.Application.Shared.Consts
{
    public class SharedMqExchanges
    {
        public const string Logs = "ex-adnc-logs";
        public const string Sms = "ex-adnc-sms";
        public const string Emails = "ex-adnc-emails";
        public const string Dead = "ex-adnc-dead-letter";
    }

    public class SharedMqRoutingKeys
    {
        public const string OpsLog = "opslog";
    }
}