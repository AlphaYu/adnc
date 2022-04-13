using Adnc.Infra.Consul.Configuration;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddConsulConfiguration(this IConfigurationBuilder configurationBuilder, ConsulConfig config, bool reloadOnChanges = false)
        {
            return configurationBuilder.Add(new DefaultConsulConfigurationSource(config, reloadOnChanges));
        }
    }
}