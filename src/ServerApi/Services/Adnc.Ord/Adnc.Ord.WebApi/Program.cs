using Adnc.Infra.Consul;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Adnc.Ord.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
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
                        //从consul配置中心读取配置
                        var consulOption = configuration.GetSection("Consul").Get<ConsulConfig>();
                        cb.AddConsulConfiguration(new[] { consulOption.ConsulUrl }, consulOption.ConsulKeyPath);
                    }
                    //cb.AddJsonFile("autofac.json", optional: true);
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
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