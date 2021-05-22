using Consul;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Infra.Consul.Consumer
{
    public class ServiceConsumer
    {
        public static async Task<List<string>> GetServicesAsync(string consulAddress, string serviceName)
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