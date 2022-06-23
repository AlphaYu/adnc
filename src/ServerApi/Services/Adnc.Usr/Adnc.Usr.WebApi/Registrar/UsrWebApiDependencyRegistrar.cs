using Adnc.Shared.WebApi.Registrar;
using Adnc.Usr.WebApi.Authentication;
using Adnc.Usr.WebApi.Authorization;

namespace Adnc.Usr.WebApi.Registrar;

public sealed class UsrWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public UsrWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault<BearerAuthenticationLocalProcessor, PermissionLocalHandler>();
        Services.AddGrpc();
    }
}