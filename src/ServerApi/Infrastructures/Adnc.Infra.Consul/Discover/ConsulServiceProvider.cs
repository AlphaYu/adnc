﻿namespace Adnc.Infra.Consul.Discover
{
    public class ConsulServiceProvider : IConsulServiceProvider
    {
        private readonly ConsulClient _consulClient;

        public ConsulServiceProvider(string uri)
        {
            _consulClient = new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(uri);
            });
        }

        public ConsulServiceProvider(Uri uri)
        {
            _consulClient = new ConsulClient(consulConfig =>
            {
                consulConfig.Address = uri;
            });
        }

        public async Task<IList<ServiceEntry>> GetAllServicesAsync(string serviceName)
        {
            var queryResult = await _consulClient.Health.Service(serviceName, string.Empty, false);
            return queryResult.Response;
        }

        public async Task<IList<string>> GetHealthServicesAsync(string serviceName)
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

    public interface IConsulServiceProvider
    {
        Task<IList<string>> GetHealthServicesAsync(string serviceName);
    }
}