namespace Adnc.Ord.WebApi;

public class Startup
{
    public void ConfigureServices(IServiceCollection services) 
        => services.GetWebApiRegistrar().AddAdnc();

    public void Configure(IApplicationBuilder app)
    {
        app.UseAdncDefault();
        app.UseRegistrationCenter();
    }
}