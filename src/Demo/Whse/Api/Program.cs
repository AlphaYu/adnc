using NLog;
using NLog.Web;

namespace Adnc.Demo.Whse.Api;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        try
        {
            LogManager.Setup().LoadConfigurationFromAppSettings();

            var startAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var startAssemblyName = startAssembly.GetName().Name ?? string.Empty;
            var lastName = startAssemblyName.Split('.').Last();
            var migrationsAssemblyName = startAssemblyName.Replace($".{lastName}", ".Migrations");
            var serviceInfo = ServiceInfo.CreateInstance(startAssembly, migrationsAssemblyName);

            //configuration,logging,webHost(kestrel)
            var builder = WebApplication.CreateBuilder(args).AddConfiguration(serviceInfo);

            //register services
            builder.Services.AddAdnc(serviceInfo, builder.Configuration);

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
            Console.WriteLine(ex);
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
}
