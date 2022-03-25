namespace Adnc.Maint.WebApi;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        await CreateHostBuilder(args)
                          .Build()
                          .ChangeThreadPoolSettings()
                          .RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configuration =>
            {
                configuration.AddCommandLine(args);
            })
            .ConfigureAppConfiguration((context, cb) =>
            {
                var env = context.HostingEnvironment;
                if (env.IsProduction() || env.IsStaging())
                {
                    var configuration = cb.Build();
                    //从consul配置中心读取配置
                    var consulOption = configuration.GetConsulSection().Get<ConsulConfig>();
                    cb.AddConsulConfiguration(consulOption, true);
                }
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureServices(services =>
            {
                services.Add(ServiceDescriptor.Singleton(typeof(IServiceInfo), ServiceInfo.Create(Assembly.GetExecutingAssembly())));
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
            })
            .UseNLog();
    }
}