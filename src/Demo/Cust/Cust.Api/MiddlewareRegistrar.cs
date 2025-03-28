﻿namespace Adnc.Demo.Cust.Api;

public sealed class MiddlewareRegistrar(IApplicationBuilder app) : AbstractWebApiMiddlewareRegistrar(app)
{
    public override void UseAdnc()
    {
        UseWebApiDefault();
    }
}

public static class WebApplicationrExtensions
{
    public static WebApplication UseAdnc(this WebApplication app)
    {
        var registrar = new MiddlewareRegistrar(app);
        registrar.UseAdnc();
        return app;
    }
}

