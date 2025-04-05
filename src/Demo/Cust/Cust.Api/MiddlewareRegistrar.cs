namespace Adnc.Demo.Cust.Api;

public sealed class MiddlewareRegistrar(WebApplication app) : AbstractWebApiMiddlewareRegistrar(app)
{
    public override void UseAdnc()
    {
        UseWebApiDefault();
    }
}

