namespace Adnc.Infra.Consul.Configuration;

public class DefaultConsulConfigurationSource(ConsulClient configClient, string consulKeyPath, bool reloadOnChanges) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new DefaultConsulConfigurationProvider(configClient, consulKeyPath, reloadOnChanges);
    }
}
