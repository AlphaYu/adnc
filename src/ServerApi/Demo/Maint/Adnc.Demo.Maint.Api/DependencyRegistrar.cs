using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Maint.Api;

public sealed class MaintWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public MaintWebApiDependencyRegistrar(IServiceCollection services) 
        : base(services)
    {
    }

    public MaintWebApiDependencyRegistrar(IApplicationBuilder app)
    : base(app)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault();
        AddHealthChecks(true, true, true, true);
        Services.AddGrpc();
    }

    public override void UseAdnc()
    {
         UseWebApiDefault(endpointRoute: endpoint =>
        {
            endpoint.MapGrpcService<Grpc.MaintGrpcServer>();
        });
    }
}