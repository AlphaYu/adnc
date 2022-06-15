using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Maint.WebApi.Registrar;

public sealed class MaintWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public MaintWebApiDependencyRegistrar(IServiceCollection services) 
        : base(services,typeof(MaintWebApiDependencyRegistrar).Assembly)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault();
        Services.AddGrpc();
    }
}