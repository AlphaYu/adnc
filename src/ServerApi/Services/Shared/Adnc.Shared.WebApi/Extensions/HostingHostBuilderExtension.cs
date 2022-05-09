using NLog.Web;

namespace Microsoft.Extensions.Hosting;

public static class HostingHostBuilderExtension
{
    public static IHostBuilder UseAdncDefault<TStartup, TRegistrar>(this IHostBuilder hostBuilder, string[] args)
    where TStartup : class
    where TRegistrar : IDependencyRegistrar
    {
        if (hostBuilder is null) throw new ArgumentNullException(nameof(hostBuilder));

        var startupAssembly = typeof(TStartup).Assembly;
        return hostBuilder
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
                        var serviceInfoInstance = ServiceInfo.GetInstance(startupAssembly);
                        services.Add(ServiceDescriptor.Singleton(typeof(IServiceInfo), serviceInfoInstance));
                        var registrarInstance = Activator.CreateInstance(typeof(TRegistrar), services);
                        services.Add(ServiceDescriptor.Singleton(typeof(IDependencyRegistrar), registrarInstance));
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<TStartup>();
                        webBuilder.ConfigureKestrel((context, serverOptions) => serverOptions.Configure(context.Configuration.GetKestrelSection(), true));
                    })
                    .ConfigureLogging((context, logging) => logging.ClearProviders().AddConsole().AddDebug())
                    .UseNLog();
    }

    //internal static async Task AspNet6(string[] args)
    //{
    //    var currentAssembly = Assembly.GetExecutingAssembly();
    //    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    //    {
    //        Args = args
    //        ,
    //        ApplicationName = currentAssembly.FullName
    //    });
    //    //add configuration
    //    builder.Services.ReplaceConfiguration(builder.Configuration);
    //    builder.Configuration.AddCommandLine(args);
    //    if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
    //    {
    //        var consulOption = builder.Configuration.GetConsulSection().Get<ConsulConfig>();
    //        builder.Configuration.AddConsulConfiguration(consulOption, true);
    //    }
    //    //add services to ms container
    //    builder.Services.AddSingleton<IServiceInfo>(ServiceInfo.Create(currentAssembly));
    //    builder.Services.AddAdncServices<PermissionHandlerLocal>();
    //    builder.Logging.ClearProviders();
    //    builder.Logging.AddConsole();
    //    builder.Logging.AddDebug();
    //    builder.Host.UseNLog();
    //    //add services to autofac container
    //    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    //    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    //    {
    //        containerBuilder.RegisterAdncModules(builder.Services);
    //    });
    //    //configure the HTTP request pipeline.
    //    var app = builder.Build();
    //    app.ChangeThreadPoolSettings();
    //    app.UseAdncMiddlewares();
    //    if (app.Environment.IsProduction() || app.Environment.IsStaging())
    //    {
    //        app.RegisterToConsul();
    //    }
    //    //run application server
    //    await app.RunAsync();
    //}
}