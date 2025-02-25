namespace Adnc.Infra.Consul.Discover.Balancers;

internal class RandomLoadBalancer : ILoadBalancer
{
    private readonly Random _random = new();

    public string Resolve(IList<string> services)
    {
        var index = _random.Next(services.Count);
        return services[index];
    }
}