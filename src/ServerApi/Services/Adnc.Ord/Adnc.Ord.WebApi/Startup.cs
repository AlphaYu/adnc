namespace Adnc.Ord.WebApi;

public class Startup
{
    public void ConfigureServices(IServiceCollection services) => services.GetWebApiRegistrar().AddAdncServices();

    public void Configure(IApplicationBuilder app, IHostEnvironment hostEnvironment)
    {
        app.UseAdncMiddlewares();

        if (hostEnvironment.IsProduction() || hostEnvironment.IsStaging())
        {
            app.RegisterToConsul();
        }
    }
}