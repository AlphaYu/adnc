namespace Adnc.Whse.WebApi;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        await CreateHostBuilder(args)
                          .Build()
                          .ChangeThreadPoolSettings()
                          .RunAsync();
    }

    internal static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
          .ConfigureHostConfiguration(configuration => configuration.AddCommandLine(args))
          .ConfigureAppConfiguration((context, cb) =>
          {
              var env = context.HostingEnvironment;
              if (env.IsProduction() || env.IsStaging())
              {
                  var configuration = cb.Build();
                  var consulOption = configuration.GetConsulSection().Get<ConsulConfig>();
                  cb.AddConsulConfiguration(consulOption, true);
              }
          })
          .UseServiceProviderFactory(new AutofacServiceProviderFactory())
          .ConfigureServices(services => services.Add(ServiceDescriptor.Singleton(typeof(IServiceInfo), ServiceInfo.GetInstance())))
          .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
          .ConfigureLogging((context, logging) => logging.ClearProviders().AddConsole().AddDebug())
          .UseNLog();
    }
}