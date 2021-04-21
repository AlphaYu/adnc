using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Consul;

namespace Adnc.Infra.Consul.Consumer
{
    public class ConsulServiceProvider : IServiceProvider
    {
        private readonly ConsulClient _consulClient;
        public ConsulServiceProvider(Uri uri)
        {
            _consulClient = new ConsulClient(consulConfig =>
            {
                consulConfig.Address = uri;
            });
        }
        public async Task<IList<string>> GetServicesAsync(string serviceName)
        {
            var queryResult = await _consulClient.Health.Service(serviceName, string.Empty, true);
            var result = new List<string>();
            foreach (var item in queryResult.Response)
            {
                result.Add($"{item.Service.Address}:{item.Service.Port}");
            }
            return result;
        }
    }
}
