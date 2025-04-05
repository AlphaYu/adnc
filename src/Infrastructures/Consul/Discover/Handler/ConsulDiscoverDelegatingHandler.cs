using Adnc.Infra.Consul.Discover.Balancers;

namespace Adnc.Infra.Consul.Discover.Handler;

public class ConsulDiscoverDelegatingHandler(ConsulClient consulClient, ILogger<ConsulDiscoverDelegatingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var currentUri = request.RequestUri ?? throw new InvalidDataException("RequestUri is null");
        var discoverProvider = new DiscoverProviderBuilder(consulClient)
                                                        .WithCacheSeconds(5)
                                                        .WithServiceName(currentUri.Host)
                                                        .WithLoadBalancer(TypeLoadBalancer.RandomLoad)
                                                        .WithLogger(logger)
                                                        .Build()
                                                        ;
        var baseUri = await discoverProvider.GetSingleHealthServiceAsync();
        if (baseUri.IsNullOrWhiteSpace())
        {
            throw new ArgumentNullException($"{currentUri.Host} does not contain helath service address!");
        }
        else
        {
            var realRequestUri = new Uri($"{currentUri.Scheme}://{baseUri}{currentUri.PathAndQuery}");
            request.RequestUri = realRequestUri;
            logger.LogDebug("RequestUri:{realRequestUri}", realRequestUri);
        }

        try
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger?.LogDebug(ex, "Exception during SendAsync()");
            throw;
        }
        finally
        {
            request.RequestUri = currentUri;
        }
    }
}
