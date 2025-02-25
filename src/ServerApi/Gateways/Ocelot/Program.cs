using Adnc.Infra.Consul.Configuration;

namespace Adnc.Gateway.Ocelot;

internal class Program
{
    internal static async Task Main(string[] args)
        => await Host.CreateDefaultBuilder(args)
                        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                        .ConfigureAppConfiguration((context, config) =>
                        {
                            var env = context.HostingEnvironment;
                            if (env.IsDevelopment())
                                config.AddJsonFile($"{AppContext.BaseDirectory}/Config/ocelot.direct.json", true, true);
                            else
                            {
                                var configuration = config.Build();
                                var consulOption = configuration.GetSection("Consul").Get<ConsulOptions>() ?? throw new ArgumentException("Consul configuration is missing");
                                config.AddConsulConfiguration(consulOption, true);
                            }
                        })
                        .Build()
                        .ChangeThreadPoolSettings()
                        .RunAsync();
}
