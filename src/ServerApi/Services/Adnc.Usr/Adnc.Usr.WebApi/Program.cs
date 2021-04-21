using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Autofac.Extensions.DependencyInjection;
using Adnc.Infra.Consul;

namespace Adnc.Usr.WebApi
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
                    var consulOption = configuration.GetSection("Consul").Get<ConsulConfig>();
                    cb.AddConsulConfiguration(consulOption, true);
                }
            });
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
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


            //return Host.CreateDefaultBuilder(args)
            //    .ConfigureHostConfiguration(configuration =>
            //    {
            //        configuration.AddCommandLine(args);
            //    })
            //    .ConfigureAppConfiguration((context, cb) =>
            //    {
            //        var env = context.HostingEnvironment;
            //        if (env.IsProduction() || env.IsStaging())
            //        {
            //            var configuration = cb.Build();
            //            //从consul配置中心读取配置
            //            var consulOption = configuration.GetSection("Consul").Get<ConsulConfig>();
            //            cb.AddConsulConfiguration(consulOption, true);
            //        }
            //        //cb.AddJsonFile("autofac.json", optional: true);
            //    })
            //    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            //    .ConfigureWebHostDefaults(webBuilder =>
            //    {
            //        webBuilder.UseStartup<Startup>();
            //    })
            //    .ConfigureLogging((context, logging) =>
            //    {
            //        logging.ClearProviders();
            //        logging.AddConsole();
            //        logging.AddDebug();
            //    })
            //    .UseNLog();
        }
    }
}