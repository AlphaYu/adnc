using Consul;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Adnc.Infr.Consul.Consumer
{

    public class ServiceConsumer : IServiceConsumer
    {
        private readonly string consulAddress;
        public ServiceConsumer(IOptionsSnapshot<ConsulOption> serviceOptions)
        {
            consulAddress = serviceOptions.Value.ConsulUrl;
        }

        public async Task<List<string>> GetServices(string serviceName)
        {
            var consulClient = new ConsulClient(configuration =>
            {
                configuration.Address = new Uri(consulAddress);
            });

            var result = await consulClient.Catalog.Service(serviceName);
            if (result == null || result.Response == null || result.Response.Length < 1)
                return null;

            var list = new List<string>();
            foreach (var item in result.Response)
            {
                list.Add($"{item.ServiceAddress}:{item.ServicePort}");
            }
            return list;
        }
    }
}