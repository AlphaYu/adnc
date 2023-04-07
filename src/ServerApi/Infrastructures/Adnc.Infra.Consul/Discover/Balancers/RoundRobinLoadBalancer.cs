namespace Adnc.Infra.Consul.Discover.Balancers;

internal class RoundRobinLoadBalancer : ILoadBalancer
{
    private readonly object _lock = new();
    private int _index = 0;

    public string Resolve(IList<string> services)
    {
        lock (_lock)
        {
            if (_index >= services.Count)
            {
                _index = 0;
            }
            return services[_index++];
        }
    }
}