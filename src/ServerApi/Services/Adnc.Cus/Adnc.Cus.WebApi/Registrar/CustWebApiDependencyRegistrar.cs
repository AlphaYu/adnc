using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Cus.WebApi.Registrar;

public sealed class CustWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public CustWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {
    }

    public override void AddAdnc() => AddWebApiDefault();
}