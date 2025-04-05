using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Ord.Api;

public sealed class MiddlewareRegistrar(WebApplication app) : AbstractWebApiMiddlewareRegistrar(app)
{
    public override void UseAdnc()
    {
        UseWebApiDefault();
    }
}
