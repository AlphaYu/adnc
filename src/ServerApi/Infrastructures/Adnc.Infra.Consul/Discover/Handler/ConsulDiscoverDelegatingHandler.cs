namespace Adnc.Infra.Consul.Discover.Handler
{
    public class ConsulDiscoverDelegatingHandler : DelegatingHandler
    {
        private readonly IServiceBuilder _serviceBuilder;
        private readonly ILogger<ConsulDiscoverDelegatingHandler> _logger;

        public ConsulDiscoverDelegatingHandler(IServiceBuilder serviceBuilder, ILogger<ConsulDiscoverDelegatingHandler> logger)
        {
            _serviceBuilder = serviceBuilder;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentUri = request.RequestUri;
            if (currentUri is null)
                throw new ArgumentNullException(nameof(request.RequestUri));

            var baseUri = await _serviceBuilder
                                                            .WithUriScheme(currentUri.Scheme)
                                                            .WithServiceName(currentUri.Host)
                                                            .WithLoadBalancer(TypeLoadBalancer.RandomLoad)
                                                            .BuildAsync()
                                                            ;
            if (baseUri is null)
                throw new ArgumentNullException($"{currentUri.Host} does not contain helath service address!");
            else
                request.RequestUri = new Uri(baseUri, currentUri.PathAndQuery);

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