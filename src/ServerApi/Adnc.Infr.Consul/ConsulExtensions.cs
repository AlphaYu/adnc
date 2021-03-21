using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Consul;
using Adnc.Infr.Consul.Configuration;
using Adnc.Infr.Consul.Consumer;
using Adnc.Infr.Consul.Registration;

namespace Adnc.Infr.Consul
{

    public static class ConsulExtensions
    {
        public static void RegisterToConsul(this IApplicationBuilder app)
        {
            ConsulRegistration.Register(app);
        }

        public static Uri GetServiceAddress(this IApplicationBuilder app, ConsulConfig config)
        {
            return ConsulRegistration.GetServiceAddress(app, config);
        }


        public static IConfigurationBuilder AddConsulConfiguration(this IConfigurationBuilder configurationBuilder, ConsulConfig config, bool reloadOnChanges = false)
        {
            return configurationBuilder.Add(new DefaultConsulConfigurationSource(config, reloadOnChanges));
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
