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
        services.AddAdncServices<PermissionHandlerRemote>();
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