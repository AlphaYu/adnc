using NLog;
using NLog.Web;

namespace Adnc.Maint.WebApi;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        var webApiAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        var serviceInfo = Shared.WebApi.ServiceInfo.CreateInstance(webApiAssembly);
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug($"init {nameof(Program.Main)}");
        try
        {
            var app = WebApplication
                .CreateBuilder(args)
                .ConfigureAdncDefault(args, serviceInfo)
                .Build();

            app.UseAdncDefault(endpointRoute: endpoint =>
            {
                endpoint.MapGrpcService<Grpc.MaintGrpcServer>();
            });

            await app
                .ChangeThreadPoolSettings()
                .UseRegistrationCenter()
                .RunAsync();
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