using Adnc.Infra.Consul.Discover.Balancers;

namespace Adnc.Infra.Consul.Discover.Handler
{
    public class ConsulDiscoverDelegatingHandler : DelegatingHandler
    {
        private readonly ConsulClient _consulClient;
        private readonly ILogger<ConsulDiscoverDelegatingHandler> _logger;

        public ConsulDiscoverDelegatingHandler(ConsulClient consulClient, ILogger<ConsulDiscoverDelegatingHandler> logger)
        {
            _consulClient = consulClient;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentUri = request.RequestUri;
            if (currentUri is null)
                throw new NullReferenceException(nameof(request.RequestUri));

            var discoverProvider = new DiscoverProviderBuilder(_consulClient)
                                                            .WithCacheSeconds(5)
                                                            .WithServiceName(currentUri.Host)
                                                            .WithLoadBalancer(TypeLoadBalancer.RandomLoad)
                                                            .Build()
                                                            ;
            var baseUri = await discoverProvider.GetSingleHealthServiceAsync();
            if (baseUri.IsNullOrWhiteSpace())
                throw new NullReferenceException($"{currentUri.Host} does not contain helath service address!");
            else
                request.RequestUri = new Uri($"{currentUri.Scheme}://{baseUri}/{currentUri.PathAndQuery}");

            try
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "Exception during SendAsync()");
                throw;
            }
            finally
            {
                request.RequestUri = currentUri;
            }
        }
    }
}