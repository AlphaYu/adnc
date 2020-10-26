using Adnc.Application.Shared;

namespace Adnc.Usr.Application
{
    public sealed class MqExchanges : BaseMqExchanges
    {
    }

    public sealed class MqRoutingKeys : BaseMqRoutingKeys
    {
        public const string Loginlog = "loginlog";
    }
}
