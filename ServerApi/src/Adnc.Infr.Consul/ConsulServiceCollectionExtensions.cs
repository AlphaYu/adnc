using Adnc.Infr.Consul.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Adnc.Infr.Consul
{

    public static class ConsulServiceCollectionExtensions
    {
        public static void AddConsulConsumer(this IServiceCollection service)
        {
            service.AddSingleton<IServiceConsumer, ServiceConsumer>();
        }
    }
}
