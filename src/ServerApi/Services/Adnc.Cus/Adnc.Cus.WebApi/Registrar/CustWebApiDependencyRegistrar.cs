﻿using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Cus.WebApi.Registrar;

public sealed class CustWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public CustWebApiDependencyRegistrar(IServiceCollection services)
        : base(services, typeof(CustWebApiDependencyRegistrar).Assembly)
    {
    }

    public override void AddAdnc() => AddWebApiDefault();
}