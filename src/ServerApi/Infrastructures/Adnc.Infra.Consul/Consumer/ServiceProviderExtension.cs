namespace Adnc.Infra.Consul.Consumer
{
    public static class ServiceProviderExtension
    {
        public static IServiceBuilder CreateServiceBuilder(this IConsulServiceProvider serviceProvider, Action<IServiceBuilder> config)
        {
            var builder = new ServiceBuilder(serviceProvider);
            config(builder);
            return builder;
        }
    }
}