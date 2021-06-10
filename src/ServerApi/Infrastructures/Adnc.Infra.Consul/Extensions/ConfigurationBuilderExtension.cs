using Adnc.Infra.Consul.Configuration;
using Adnc.Infra.Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddConsulConfiguration(this IConfigurationBuilder configurationBuilder, ConsulConfig config, bool reloadOnChanges = false)
        {
            return configurationBuilder.Add(new DefaultConsulConfigurationSource(config, reloadOnChanges));
        }
    }
}