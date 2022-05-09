using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Usr.WebApi.Registrar;

public sealed class UsrWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public UsrWebApiDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault<PermissionHandlerLocal>();
        Services.AddGrpc();
    }
}