namespace Adnc.Usr.WebApi;

public class Startup
{
    private IServiceCollection _services;

    public void ConfigureServices(IServiceCollection services)
    {
        _services = services;
        services.AddAdncServices<PermissionHandlerLocal>();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterAdncModules(_services);
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment environment)
    {
        app.UseAdncMiddlewares();

        if (environment.IsProduction() || environment.IsStaging())
        {
            app.RegisterToConsul();
        }
    }
}