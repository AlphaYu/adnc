using Adnc.Ord.Application.EventSubscribers;

namespace Adnc.Ord.WebApi;

public class Startup
{
    private readonly IHostEnvironment _environment;
    private IServiceCollection _services;

    public Startup(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        _services = services;
        services.AddAdncServices<PermissionHandlerRemote>(registion =>
        {
            var policies = registion.GenerateDefaultRefitPolicies();
            var authServeiceAddress = _environment.IsDevelopment() ? "http://localhost:5010" : "adnc.usr.webapi";
            registion.AddRpcService<IAuthRpcService>(authServeiceAddress, policies);

            var maintServiceAddress = _environment.IsDevelopment() ? "http://localhost:5020" : "adnc.maint.webapi";
            registion.AddRpcService<IMaintRpcService>(maintServiceAddress, policies);

            var whseServiceAddress = _environment.IsDevelopment() ? "http://localhost:8065" : "adnc.whse.webapi";
            registion.AddRpcService<IWhseRpcService>(whseServiceAddress, policies);

            registion.AddEventBusSubscribers<CapEventSubscriber>();
        });
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterAdncModules(_services);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseAdncMiddlewares();

        if (_environment.IsProduction() || _environment.IsStaging())
        {
            app.RegisterToConsul();
        }
    }
}