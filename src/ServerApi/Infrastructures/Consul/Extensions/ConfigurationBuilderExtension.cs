using Adnc.Infra.Consul.Configuration;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddConsulConfiguration(this IConfigurationBuilder configurationBuilder, ConsulOptions config, bool reloadOnChanges = false)
        {
            var consulClient = new ConsulClient(client => client.Address = new Uri(config.ConsulUrl));
            var pathKeys = config.ConsulKeyPath.Split(",", StringSplitOptions.RemoveEmptyEntries);
            foreach (var pathKey in pathKeys)
            {
                configurationBuilder.Add(new DefaultConsulConfigurationSource(consulClient, pathKey, reloadOnChanges));
            }
            return configurationBuilder;
        }
    }
}