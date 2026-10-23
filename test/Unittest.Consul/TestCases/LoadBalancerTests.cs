using Adnc.Infra.Consul.Discover.Balancers;

namespace Adnc.Infra.Unittest.Consul.TestCases;

public class LoadBalancerTests
{
    [Fact]
    public void RandomLoadBalancerReturnsEmptyWhenNoServicesAreHealthy()
    {
        Assert.Equal(string.Empty, TypeLoadBalancer.RandomLoad.Resolve(Array.Empty<string>()));
    }

    [Fact]
    public void RoundRobinLoadBalancerReturnsEmptyWhenNoServicesAreHealthy()
    {
        Assert.Equal(string.Empty, TypeLoadBalancer.RoundRobinLoad.Resolve(Array.Empty<string>()));
    }
}
