using Grpc.Net.Client.Balancer;

namespace Adnc.Infra.Consul.Discover.GrpcResolver;

//https://docs.microsoft.com/zh-cn/aspnet/core/grpc/loadbalancing?view=aspnetcore-6.0
public sealed class ConsulGrpcResolver(Uri uri, ConsulClient client, ILoggerFactory loggerFactory) : PollingResolver(loggerFactory)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ConsulGrpcResolver>();
    private readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(30);
    private Timer? _timer;

    protected override async Task ResolveAsync(CancellationToken cancellationToken)
    {
        var address = uri.Host.Replace("consul://", string.Empty);
        var _consulServiceProvider = new DiscoverProviderBuilder(client).WithServiceName(address).WithCacheSeconds(5).Build();
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
            _logger.LogError(ex, "ConsulGrpcResolver.OnTimerCallback");
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        _timer?.Dispose();
    }
}

public class ConsulGrpcResolverFactory(ConsulClient consulClient) : ResolverFactory
{
    public override string Name => "consul";

    public override Resolver Create(ResolverOptions options) => new ConsulGrpcResolver(options.Address, consulClient, options.LoggerFactory);
}
