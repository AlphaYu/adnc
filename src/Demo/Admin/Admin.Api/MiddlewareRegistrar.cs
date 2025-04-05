using Adnc.Demo.Admin.Api.Grpc;
using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Admin.Api;

public sealed class MiddlewareRegistrar(WebApplication app) : AbstractWebApiMiddlewareRegistrar(app)
{
    public override void UseAdnc()
    {
        UseWebApiDefault(endpointRoute: endpoint =>
        {
            endpoint.MapGrpcService<AdminGrpcServer>();
        });
    }
}
