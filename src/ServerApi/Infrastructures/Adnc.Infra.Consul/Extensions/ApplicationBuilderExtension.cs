using Adnc.Infra.Consul;
using Adnc.Infra.Consul.Configuration;
using Adnc.Infra.Consul.Registration;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtension
    {
        public static void RegisterToConsul(this IApplicationBuilder app)
        {
            ConsulRegistration.Register(app);
        }

        public static Uri GetServiceAddress(this IApplicationBuilder app, ConsulConfig config)
        {
            return ConsulRegistration.GetServiceAddress(app, config);
        }
    }
}