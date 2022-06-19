namespace Adnc.Infra.Consul.Discover
{
    public static class ServiceProviderExtension
    {
        public static IServiceBuilder CreateServiceBuilder(this IConsulServiceProvider serviceProvider)  => new ServiceBuilder(serviceProvider);

        public static IServiceBuilder WithLoadBalancer(this IServiceBuilder buider, ILoadBalancer balancer)
        {
            if (balancer is null)
                throw new ArgumentNullException(nameof(balancer));
            buider.LoadBalancer = balancer;
            return buider;
        }

        public static IServiceBuilder WithUriScheme(this IServiceBuilder buider, string uriScheme)
        {
            if (uriScheme.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(uriScheme));
            buider.UriScheme = uriScheme;
            return buider;
        }

        public static IServiceBuilder WithServiceName(this IServiceBuilder buider, string serviceName)
        {
            if (serviceName.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(serviceName));
            buider.ServiceName = serviceName;
            return buider;
        }

        public static async Task<Uri> BuildAsync(this IServiceBuilder buider) => await BuildAsync(buider, null);

        public static async Task<Uri> BuildAsync(this IServiceBuilder buider, string? pathAndQuery)
        {
            var serviceList = await buider.ServiceProvider.GetHealthServicesAsync(buider.ServiceName);
            if(buider.LoadBalancer is null)
                buider.LoadBalancer = TypeLoadBalancer.RandomLoad;
            var service = buider.LoadBalancer.Resolve(serviceList);
            var baseUri = new Uri($"{buider.UriScheme}://{service}");
            var uri = new Uri(baseUri, pathAndQuery);
            return uri;
        }
    }
}