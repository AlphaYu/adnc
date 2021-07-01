using Adnc.Infra.Consul;
using Adnc.Infra.Core;
using Adnc.WebApi.Shared;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.Reflection;

namespace Adnc.Whse.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();
            host.Run();
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
}