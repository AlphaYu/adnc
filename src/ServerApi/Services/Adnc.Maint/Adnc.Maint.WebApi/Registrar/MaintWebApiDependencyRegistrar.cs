using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Maint.WebApi.Registrar;

public sealed class MaintWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public MaintWebApiDependencyRegistrar(IServiceCollection services) 
        : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault();
        AddHealthChecks(true, true, true, true);
        Services.AddGrpc();
    }
}