using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Admin.Api
{
    public sealed class MiddlewareRegistrar(IApplicationBuilder app) : AbstractWebApiMiddlewareRegistrar(app)
    {
        public override  void  UseAdnc()
        {
            UseWebApiDefault(endpointRoute: endpoint =>
            {
                endpoint.MapGrpcService<Grpc.AuthGrpcServer>();
                endpoint.MapGrpcService<Grpc.UsrGrpcServer>();
            });
        }
    }

    public static class WebApplicationrExtensions
    {
        public static WebApplication UseAdnc(this WebApplication app)
        {
            var registrar = new MiddlewareRegistrar(app);
            registrar.UseAdnc();
            return app;
        }
    }
}
