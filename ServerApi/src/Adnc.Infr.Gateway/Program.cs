using System;
using Adnc.Infr.Consul.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Adnc.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                            .ConfigureAppConfiguration((context, cb) =>
                            {
                                var env = context.HostingEnvironment;
                                if (env.IsProduction() || env.IsStaging())
                                {
                                    //生产环境从consul读取配置
                                    var configuration = cb.Build();
                                    cb.AddConsul(new[] { configuration.GetValue<Uri>("ConsulUrl") }, configuration.GetValue<string>("ConsulKeyPath"));
                                }
                            })
                            .ConfigureAppConfiguration((hostingContext, config) =>
                            {
                                var env = hostingContext.HostingEnvironment;
                                config.AddJsonFile($"{AppContext.BaseDirectory}/Config/ocelot.{env.EnvironmentName}.json", false, true);
                            })
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseStartup<Startup>();
                            });
    }
}
