using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Cus.WebApi.Registrar;

public sealed class CustWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public CustWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {
    }

    public CustWebApiDependencyRegistrar(IApplicationBuilder app)
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