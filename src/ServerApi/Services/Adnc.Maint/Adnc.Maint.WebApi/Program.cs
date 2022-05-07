using Adnc.Maint.WebApi.Registrar;

namespace Adnc.Maint.WebApi;

internal static class Program
{
    internal static async Task Main(string[] args) =>
    await CreateHostBuilder(args)
              .Build()
              .ChangeThreadPoolSettings()
              .RunAsync();

    internal static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
            .UseAdncDefault<Startup, MaintWebApiDependencyRegistrar>(args);
}