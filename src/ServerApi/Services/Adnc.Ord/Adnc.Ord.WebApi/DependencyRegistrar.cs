﻿using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Ord.WebApi;

public sealed class OrdWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public OrdWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {
    }

    public OrdWebApiDependencyRegistrar(IApplicationBuilder app)
    : base(app)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault();
        AddHealthChecks(true, true, true, true);
    }

    public override void UseAdnc()
    {
        UseWebApiDefault();
    }
}