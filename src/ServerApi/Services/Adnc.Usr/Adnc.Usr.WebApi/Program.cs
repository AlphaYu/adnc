namespace Adnc.Usr.WebApi;

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
          .ConfigureServices(services => 
          {
              services.Add(ServiceDescriptor.Singleton(typeof(IServiceInfo), ServiceInfo.GetInstance(typeof(Program).Assembly)));
              services.Add(ServiceDescriptor.Singleton(typeof(IDependencyRegistrar), new Registrar.UsrWebApiDependencyRegistrar(services)));
          })
          .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
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