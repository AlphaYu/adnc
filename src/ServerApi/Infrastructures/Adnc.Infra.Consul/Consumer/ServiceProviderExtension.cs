using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Infra.Consul.Consumer
{
    public static class ServiceProviderExtension
    {
        public static IServiceBuilder CreateServiceBuilder(this IServiceProvider serviceProvider, Action<IServiceBuilder> config)
        {
            var builder = new ServiceBuilder(serviceProvider);
            config(builder);
            return builder;
        }
    }
}
