using Adnc.Infra.Consul.Discover;
using Adnc.Infra.Consul.Discover.Balancers;
using Consul;

namespace Adnc.Infra.Unittest.Consul.TestCases;

public class ConsulTests : IClassFixture<ConsulContextFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly ConsulClient _consulClient;

    public ConsulTests(ConsulContextFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _consulClient = fixture.Container.GetRequiredService<ConsulClient>();
      //  _loggerFactory = fixture.Container.GetRequiredService<ILoggerFactory>();
    }

    /// <summary>
    /// 测试服务发现
    /// </summary>
    [Fact]
    public async Task TestDiscover()
    {
       // var logger = _loggerFactory.CreateLogger<dynamic>();
        var discoverProvider = new DiscoverProviderBuilder(_consulClient)
                                                            .WithCacheSeconds(5)
                                                            .WithServiceName("adnc-demo-admin-api")
                                                            .WithLoadBalancer(TypeLoadBalancer.RandomLoad)
                                                            .WithLogger(null)
                                                            .Build();

        var services = await discoverProvider.GetAllHealthServicesAsync();
        Assert.NotNull(services);
    }
}
