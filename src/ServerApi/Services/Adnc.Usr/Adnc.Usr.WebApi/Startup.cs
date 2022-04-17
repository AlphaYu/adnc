namespace Adnc.Usr.WebApi;

public class Startup
{
    public void ConfigureServices(IServiceCollection services) => services.GetWebApiRegistrar().AddAdncServices();

    public void Configure(IApplicationBuilder app, IHostEnvironment environment)
    {
        app.UseAdncMiddlewares();

        if (environment.IsProduction() || environment.IsStaging())
        {
            app.RegisterToConsul();
        }
    }
}