using Adnc.Application.Shared;

namespace Adnc.Maint.Application.Contracts.Consts
{
    public sealed class MqExchanges: BaseMqExchanges
    {
    }

    public sealed class MqRoutingKeys: BaseMqRoutingKeys
    {
        public const string Loginlog = "loginlog";
    }
}
