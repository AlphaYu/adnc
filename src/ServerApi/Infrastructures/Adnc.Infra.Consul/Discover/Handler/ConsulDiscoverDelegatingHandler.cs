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
            if(request.RequestUri.Host.Contains('.'))
                return await base.SendAsync(request, cancellationToken);

            var baseUri = await _serviceBuilder
                                                            .WithUriScheme(request.RequestUri.Scheme)
                                                            .WithServiceName(request.RequestUri.Host)
                                                            .WithLoadBalancer(TypeLoadBalancer.RandomLoad)
                                                            .BuildAsync()
                                                            ;
            if (baseUri is null)
                throw new ArgumentNullException($"{request.RequestUri.Host} does not contain helath service address!");
            else
                request.RequestUri = new Uri(baseUri, request.RequestUri.PathAndQuery);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}