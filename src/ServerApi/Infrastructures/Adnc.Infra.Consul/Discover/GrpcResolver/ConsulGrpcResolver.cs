using Grpc.Net.Client.Balancer;

namespace Adnc.Infra.Consul.Discover.GrpcResolver;

//https://docs.microsoft.com/zh-cn/aspnet/core/grpc/loadbalancing?view=aspnetcore-6.0
public sealed class ConsulGrpcResolver : PollingResolver
{
    private readonly Uri _address;
    private readonly int _port;
    private readonly IConsulServiceProvider _consulServiceProvider;

    public ConsulGrpcResolver(Uri address, int defaultPort, IConsulServiceProvider consulServiceProvider, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _address = address;
        _port = defaultPort;
        _consulServiceProvider = consulServiceProvider;
    }

    protected override async Task ResolveAsync(CancellationToken cancellationToken)
    {
        var address = _address.Host.Replace("consul://", string.Empty);
        var results = await _consulServiceProvider.GetHealthServicesAsync(address);
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
}

public class ConsulGrpcResolverFactory : ResolverFactory
{
    private IConsulServiceProvider _consulServiceProvider;

    public ConsulGrpcResolverFactory(IConsulServiceProvider consulServiceProvider)
     => _consulServiceProvider = consulServiceProvider;

    public override string Name => "consul";

    public override Resolver Create(ResolverOptions options)
     => new ConsulGrpcResolver(options.Address, options.DefaultPort, _consulServiceProvider, options.LoggerFactory);
}
