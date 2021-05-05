using Adnc.Application.Shared.Consts;

namespace Adnc.Usr.Application.Contracts.Consts
{
    public sealed class MqExchanges : SharedMqExchanges
    {
    }

    public sealed class MqRoutingKeys : SharedMqRoutingKeys
    {
        public const string Loginlog = "loginlog";
    }
}
