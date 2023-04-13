using NLog;
using NLog.Web;

namespace Adnc.Demo.Cust.Api;

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
            var serviceInfo = ServiceInfo.CreateInstance(startAssembly, migrationsAssemblyName: startAssemblyName);

            var app = WebApplication
                .CreateBuilder(args)
                .ConfigureAdncDefault(serviceInfo)
                .Build();

            app.UseAdnc();

            app.ChangeThreadPoolSettings()
                .UseRegistrationCenter();

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
            LogManager.Shutdown();
        }
    }
}