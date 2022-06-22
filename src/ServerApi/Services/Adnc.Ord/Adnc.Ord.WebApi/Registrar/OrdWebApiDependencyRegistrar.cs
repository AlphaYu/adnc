using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Ord.WebApi.Registrar;

public sealed class OrdWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public OrdWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {
    }

    public override void AddAdnc() => AddWebApiDefault();
}