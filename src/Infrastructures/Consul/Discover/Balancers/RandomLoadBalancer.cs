namespace Adnc.Infra.Consul.Discover.Balancers;

internal sealed class RandomLoadBalancer : ILoadBalancer
{
    private readonly Random _random = new();

    public string Resolve(IList<string> services)
    {
        if (services.Count == 0)
        {
            return string.Empty;
        }

        var index = _random.Next(services.Count);
        return services[index];
    }
}
