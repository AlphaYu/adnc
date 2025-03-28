using NLog;
using NLog.Web;

namespace Adnc.Demo.Ord.Api;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug($"init {nameof(Program.Main)}");
        try
        {
            var startAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var startAssemblyName = startAssembly.GetName().Name ?? string.Empty;
            var lastName = startAssemblyName.Split('.').Last();
            var migrationsAssemblyName = startAssemblyName.Replace($".{lastName}", ".Migrations");
            var serviceInfo = ServiceInfo.CreateInstance(startAssembly, migrationsAssemblyName);

            //configuration,logging,webHost(kestrel)
            var builder = WebApplication.CreateBuilder(args).AddConfiguration(serviceInfo);

            //register services
            builder.Services.AddAdnc(serviceInfo);

            //create webHost
            var app = builder.Build();

            //register middlewares
            app.UseAdnc();

            //other settings
            app.ChangeThreadPoolSettings()
                .UseRegistrationCenter();

            app.MapGet("/", async context =>
            {
                var content = serviceInfo.GetDefaultPageContent(app.Services);
                context.Response.Headers.TryAdd("Content-Type", "text/html");
                await context.Response.WriteAsync(content);
            });

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception");
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
}