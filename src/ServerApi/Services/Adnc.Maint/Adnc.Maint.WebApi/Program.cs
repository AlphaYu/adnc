using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Fluent;
using NLog.Web;
using Adnc.Infra.Common;
using Adnc.Infra.Consul.Configuration;
using Adnc.Infra.Consul;

namespace Adnc.Maint.WebApi
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
                        cb.AddConsulConfiguration(consulOption, true);
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