using Microsoft.Extensions.Configuration;

namespace Adnc.Infr.Consul.Configuration
{
    public class DefaultConsulConfigurationSource : IConfigurationSource
    {
        private readonly ConsulConfig _config;
        private readonly bool _reloadOnChanges;

        public DefaultConsulConfigurationSource(ConsulConfig config,bool reloadOnChanges)
        {
            _config = config;
            _reloadOnChanges = reloadOnChanges;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DefaultConsulConfigurationProvider(_config, _reloadOnChanges);
        }
    }
}
