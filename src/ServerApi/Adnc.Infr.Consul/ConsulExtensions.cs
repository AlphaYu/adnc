using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Consul;
using Adnc.Infr.Consul.Configuration;
using Adnc.Infr.Consul.Consumer;

namespace Adnc.Infr.Consul
{

    public static class ConsulExtensions
    {
        public static void AddConsulServices(this IServiceCollection services, ConsulConfig config)
        {
            services.AddScoped<ITokenGenerator, DefaultTokenGenerator>();
            services.AddScoped<SimpleDiscoveryHandler>();
            services.AddScoped<ConsulDiscoveryHandler>();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton(new ConsulClient(cfg =>
            {
                cfg.Address = new Uri(config.ConsulUrl);
            }));
        }

        public static IConfigurationBuilder AddConsulConfiguration(this IConfigurationBuilder configurationBuilder, IEnumerable<Uri> consulUrls, string consulPath)
        {
            return configurationBuilder.Add(new ConsulConfigurationSource(consulUrls, consulPath));
        }

        public static IConfigurationBuilder AddConsulConfiguration(this IConfigurationBuilder configurationBuilder, IEnumerable<string> consulUrls, string consulPath)
        {
            return configurationBuilder.AddConsulConfiguration(consulUrls.Select(u => new Uri(u)), consulPath);
        }
    }
}
