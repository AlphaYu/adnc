namespace Adnc.Ord.WebApi;

public  static class Program
{
    public static async Task Main(string[] args)
    => await CreateHostBuilder(args)
                    .Build()
                    .ChangeThreadPoolSettings()
                    .RunAsync();

    public static IHostBuilder CreateHostBuilder(string[] args)
    => Host.CreateDefaultBuilder(args)
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
          .ConfigureServices(services =>
          {
              services.Add(ServiceDescriptor.Singleton(typeof(IServiceInfo), ServiceInfo.GetInstance(typeof(Program).Assembly)));
              services.Add(ServiceDescriptor.Singleton(typeof(IDependencyRegistrar), new Registrar.OrdWebApiDependencyRegistrar(services)));
          })
          .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
          .ConfigureLogging((context, logging) => logging.ClearProviders().AddConsole().AddDebug())
          .UseNLog();
}