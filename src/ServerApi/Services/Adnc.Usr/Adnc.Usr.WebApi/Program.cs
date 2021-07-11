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

namespace Adnc.Usr.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();
            host.ChangeThreadPoolSettings();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureHostConfiguration(configuration =>
            {
                configuration.AddCommandLine(args);
            });
            hostBuilder.ConfigureAppConfiguration((context, cb) =>
            {
                var env = context.HostingEnvironment;
                if (env.IsProduction() || env.IsStaging())
                {
                    var configuration = cb.Build();
                    var consulOption = configuration.GetConsulSection().Get<ConsulConfig>();
                    cb.AddConsulConfiguration(consulOption, true);
                }
            });
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            hostBuilder.ConfigureServices(services =>
            {
                services.Add(ServiceDescriptor.Singleton(typeof(IServiceInfo), ServiceInfo.Create(Assembly.GetExecutingAssembly())));
            });
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
            hostBuilder.ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
            });
            hostBuilder.UseNLog();

            return hostBuilder;
        }
    }
}