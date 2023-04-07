using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Cust.WebApi;

public sealed class DependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public DependencyRegistrar(IServiceCollection services)
        : base(services)
    {
    }

    public DependencyRegistrar(IApplicationBuilder app)
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