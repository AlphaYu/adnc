using Adnc.Infra.Consul.Configuration;

namespace Adnc.Gateway.Ocelot;

public class Program
{
    internal static async Task Main(string[] args)
        => await Host.CreateDefaultBuilder(args)
                        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                        .ConfigureAppConfiguration((context, configBuilder) =>
                        {
                            var configuration = configBuilder.Build();
                            var configurationType = configuration.GetValue<string>("ConfigurationType") ?? "File";
                            if (configurationType == "File")
                            {
                                configBuilder.AddJsonFile($"{AppContext.BaseDirectory}/Config/ocelot.direct.json", true, true);
                            }
                            else if (configurationType == "Consul")
                            {
                                var consulOption = configuration.GetSection("Consul").Get<ConsulOptions>() ?? throw new ArgumentException("Consul configuration is missing");
                                configBuilder.AddConsulConfiguration(consulOption, true);
                            }
                            else
                            {
                                throw new ArgumentException("ConfigurationType is invalid");
                            }
                        })
                        .Build()
                        .ChangeThreadPoolSettings()
                        .RunAsync();
}
