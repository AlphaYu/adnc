namespace Adnc.Infra.Consul.Discover
{
    public class ServiceBuilder : IServiceBuilder
    {
        public IConsulServiceProvider ServiceProvider { get; set; }
        public string ServiceName { get; set; }
        public string UriScheme { get; set; }
        public ILoadBalancer LoadBalancer { get; set; }

        public ServiceBuilder(IConsulServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public async Task<Uri> BuildAsync(string path)
        {
            var serviceList = await ServiceProvider.GetHealthServicesAsync(ServiceName);
            var service = LoadBalancer.Resolve(serviceList);
            var baseUri = new Uri($"{UriScheme}://{service}");
            var uri = new Uri(baseUri, path);
            return uri;
        }
    }

    public interface IServiceBuilder
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        IConsulServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        string ServiceName { get; set; }

        /// <summary>
        /// Uri方案
        /// </summary>
        string UriScheme { get; set; }

        /// <summary>
        /// 使用哪种策略
        /// </summary>
        ILoadBalancer LoadBalancer { get; set; }

        Task<Uri> BuildAsync(string path);
    }

}