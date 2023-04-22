using Adnc.Demo.Usr.Api.Authentication;
using Adnc.Demo.Usr.Api.Authorization;
using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Usr.Api;

public sealed class UsrWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public UsrWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {
    }

    public UsrWebApiDependencyRegistrar(IApplicationBuilder app)
        : base(app)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault<BearerAuthenticationLocalProcessor, PermissionLocalHandler>();
        AddHealthChecks(true, true, true, false);
        Services.AddGrpc();
    }

    public override void UseAdnc()
    {
        UseWebApiDefault(endpointRoute: endpoint =>
        {
            endpoint.MapGrpcService<Grpc.AuthGrpcServer>();
            endpoint.MapGrpcService<Grpc.UsrGrpcServer>();
        });
    }
}