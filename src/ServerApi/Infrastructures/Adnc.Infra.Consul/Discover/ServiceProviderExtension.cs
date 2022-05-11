namespace Adnc.Infra.Consul.Discover
{
    public static class ServiceProviderExtension
    {
        public static IServiceBuilder CreateServiceBuilder(this IConsulServiceProvider serviceProvider)
        => new ServiceBuilder(serviceProvider);

        public static IServiceBuilder ConfigLoadBalancer(this IServiceBuilder buider, ILoadBalancer balancer)
        {
            if (balancer is null)
                throw new ArgumentNullException(nameof(balancer));
            buider.LoadBalancer = balancer;
            return buider;
        }

        public static IServiceBuilder ConfigUriScheme(this IServiceBuilder buider, string uriScheme)
        {
            if (uriScheme.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(uriScheme));
            buider.UriScheme = uriScheme;
            return buider;
        }

        public static IServiceBuilder ConfigServiceName(this IServiceBuilder buider, string serviceName)
        {
            if (serviceName.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(serviceName));
            buider.ServiceName = serviceName;
            return buider;
        }

        public static async Task<Uri> BuildAsync(this IServiceBuilder buider, string path)
        {
            var serviceList = await buider.ServiceProvider.GetHealthServicesAsync(buider.ServiceName);
            var service = buider.LoadBalancer.Resolve(serviceList);
            var baseUri = new Uri($"{buider.UriScheme}://{service}");
            var uri = new Uri(baseUri, path);
            return uri;
        }
    }
}