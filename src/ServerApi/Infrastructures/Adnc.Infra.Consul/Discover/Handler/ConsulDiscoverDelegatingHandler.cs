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
                                                            .WithLogger(_logger)
                                                            .Build()
                                                            ;
            var baseUri = await discoverProvider.GetSingleHealthServiceAsync();
            if (baseUri.IsNullOrWhiteSpace())
                throw new NullReferenceException($"{currentUri.Host} does not contain helath service address!");
            else
            {
                var realRequestUri = new Uri($"{currentUri.Scheme}://{baseUri}{currentUri.PathAndQuery}");
                request.RequestUri = realRequestUri;
                _logger.LogDebug($"RequestUri:{request.RequestUri}");
            }

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