namespace Adnc.Infra.Consul.Configuration
{
    public class DefaultConsulConfigurationSource : IConfigurationSource
    {
        private readonly ConsulClient _configClient;
        private readonly string _consulKeyPath;
        private readonly bool _reloadOnChanges;

        public DefaultConsulConfigurationSource(ConsulClient configClient, string consulKeyPath, bool reloadOnChanges)
        {
            _configClient = configClient;
            _consulKeyPath = consulKeyPath;
            _reloadOnChanges = reloadOnChanges;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DefaultConsulConfigurationProvider(_configClient, _consulKeyPath, _reloadOnChanges);
        }
    }
}