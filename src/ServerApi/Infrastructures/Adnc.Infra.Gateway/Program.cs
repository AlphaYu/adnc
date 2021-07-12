using Adnc.Infra.Consul;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Adnc.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .ChangeThreadPoolSettings()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                            .ConfigureAppConfiguration((context, cb) =>
                            {
                                //生产环境从consul配置中心读取配置
                                var env = context.HostingEnvironment;
                                if (env.IsProduction() || env.IsStaging())
                                {
                                    var configuration = cb.Build();
                                    var consulOption = configuration.GetSection("Consul").Get<ConsulConfig>();
                                    cb.AddConsulConfiguration(consulOption, true);
                                }
                            })
                            .ConfigureAppConfiguration((hostingContext, config) =>
                            {
                                var env = hostingContext.HostingEnvironment;
                                config.AddJsonFile($"{AppContext.BaseDirectory}/Config/ocelot.{env.EnvironmentName}.json", true, true);
                            })
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseStartup<Startup>();
                            });
    }
}