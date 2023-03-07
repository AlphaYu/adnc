using Grpc.Net.Client.Balancer;

namespace Adnc.Infra.Consul.Discover.GrpcResolver;

//https://docs.microsoft.com/zh-cn/aspnet/core/grpc/loadbalancing?view=aspnetcore-6.0
public sealed class ConsulGrpcResolver : PollingResolver
{
    private readonly Uri _address;
    private readonly int _port;
    private readonly ConsulClient _client;
    private Timer? _timer;
    private readonly TimeSpan _refreshInterval;
    private readonly ILogger _logger;

    public ConsulGrpcResolver(Uri address, int defaultPort, ConsulClient client, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _address = address;
        _port = defaultPort;
        _client = client;
        _logger = loggerFactory.CreateLogger<ConsulGrpcResolver>();
        _refreshInterval = TimeSpan.FromSeconds(30);
    }

    protected override async Task ResolveAsync(CancellationToken cancellationToken)
    {
        var address = _address.Host.Replace("consul://", string.Empty);
        var _consulServiceProvider = new DiscoverProviderBuilder(_client).WithServiceName(address).WithCacheSeconds(5).Build();
        var results = await _consulServiceProvider.GetAllHealthServicesAsync();
        var balancerAddresses = new List<BalancerAddress>();
        results.ForEach(result =>
        {
            var addressArray = result.Split(":");
            var host = addressArray[0];
            var port = int.Parse(addressArray[1]) + 1;
            balancerAddresses.Add(new BalancerAddress(host, port));
        });
        // Pass the results back to the channel.
        Listener(ResolverResult.ForResult(balancerAddresses));
    }

    protected override void OnStarted()
    {
        base.OnStarted();

        if (_refreshInterval != Timeout.InfiniteTimeSpan)
        {
            _timer = new Timer(OnTimerCallback, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            _timer.Change(_refreshInterval, _refreshInterval);
        }
    }

    private void OnTimerCallback(object? state)
    {
        try
        {
            Refresh();
        }
        catch (Exception ex)
        {
            _logger.LogError("ConsulGrpcResolver.OnTimerCallback", ex);
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        _timer?.Dispose();
    }

}

public class ConsulGrpcResolverFactory : ResolverFactory
{
    private ConsulClient _consulClient;

    public ConsulGrpcResolverFactory(ConsulClient consulClient) => _consulClient = consulClient;

    public override string Name => "consul";

    public override Resolver Create(ResolverOptions options) => new ConsulGrpcResolver(options.Address, options.DefaultPort, _consulClient, options.LoggerFactory);
}