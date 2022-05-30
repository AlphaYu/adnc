namespace Adnc.Maint.WebApi;

public class Startup
{
    public void ConfigureServices(IServiceCollection services) 
        => services.GetWebApiRegistrar().AddAdnc();

    public void Configure(IApplicationBuilder app)
    {
        app.UseAdncDefault(endpointRoute: endpoint => endpoint.MapGrpcService<Grpc.MaintGrpcServer>());
        app.UseRegistrationCenter();
    }
}