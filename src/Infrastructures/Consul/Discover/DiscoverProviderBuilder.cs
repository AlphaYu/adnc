using Adnc.Infra.Consul.Discover.Balancers;

namespace Adnc.Infra.Consul.Discover;

public class DiscoverProviderBuilder
{
    private readonly DiscoverProvider _discoverProvider = default!;

    public DiscoverProviderBuilder(ConsulClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _discoverProvider = new DiscoverProvider(client);
    }

    public DiscoverProviderBuilder WithLoadBalancer(ILoadBalancer balancer)
    {
        _discoverProvider.LoadBalancer = balancer;
        return this;
    }

    public DiscoverProviderBuilder WithServiceName(string serviceName)
    {
        _discoverProvider.ServiceName = serviceName;
        return this;
    }

    public DiscoverProviderBuilder WithCacheSeconds(uint seconds)
    {
        _discoverProvider.CacheSeconds = seconds;
        return this;
    }

    public DiscoverProviderBuilder WithLogger(ILogger<dynamic> logger)
    {
        _discoverProvider.Logger = logger;
        return this;
    }

    public IDiscoverProvider Build()
    {
        if (_discoverProvider.ServiceName is null)
        {
            throw new ArgumentNullException(nameof(_discoverProvider.ServiceName));
        }

        if (_discoverProvider.CacheSeconds == 0)
        {
            _discoverProvider.CacheSeconds = 5;
        }

        _discoverProvider.LoadBalancer ??= TypeLoadBalancer.RandomLoad;

        return _discoverProvider;
    }
}
