using NLog;
using NLog.Web;

namespace Adnc.Usr.WebApi;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug($"init {nameof(Program.Main)}");
        try
        {
            var webApiAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var serviceInfo = Shared.WebApi.ServiceInfo.CreateInstance(webApiAssembly);

            //Configuration,ServiceCollection,Logging,WebHost(Kestrel)
            var app = WebApplication
                .CreateBuilder(args)
                .ConfigureAdncDefault(serviceInfo)
                .Build();

            //Middlewares
            app.UseAdnc();

            //Start
            app.ChangeThreadPoolSettings()
                .UseRegistrationCenter();

            //Default page
            app.MapGet("/", async context =>
            {
                var content = serviceInfo.GetDefaultPageContent(app.Services);
                context.Response.Headers.Add("Content-Type", "text/html");
                await context.Response.WriteAsync(content);
            });

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception");
            throw;
        }
        finally
        {
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            LogManager.Shutdown();
        }
    }
}