namespace Adnc.Application.Shared
{
    public class BaseMqExchanges
    {
        public const string Logs = "ex-adnc-logs";
        public const string Sms = "ex-adnc-sms";
        public const string Emails = "ex-adnc-emails";
        public const string Dead = "ex-adnc-dead-letter";
    }

    public class BaseMqRoutingKeys
    {
        public const string OpsLog = "opslog";
    }
}
