using Adnc.Infra.Consul.Configuration;
using Consul;
using Microsoft.Extensions.Options;

namespace Adnc.Infra.Unittest.Consul.Fixtures;

public class ConsulContextFixture
{
    public IServiceProvider Container { get; private set; }
    public IConfiguration Configuration { get; private set; }

    public ConsulContextFixture()
    {
        Configuration = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: true)
                                            .Build();

        var consulSection = Configuration.GetSection("Consul");

        var services = new ServiceCollection();
            services.Configure<ConsulOptions>(consulSection)
                .AddSingleton(provider =>
                {
                    var configOptions = provider.GetService<IOptions<ConsulOptions>>();
                    if (configOptions is null)
                        throw new NullReferenceException(nameof(configOptions));
                    return new ConsulClient(x => x.Address = new Uri(configOptions.Value.ConsulUrl));
                });


        Container = services.BuildServiceProvider();
    }
}
