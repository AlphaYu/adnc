using Adnc.Infra.Consul.Discover.Balancers;
using Adnc.Infra.Core.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;

namespace Adnc.Infra.Consul.Discover
{
    internal class DiscoverProvider : IDiscoverProvider
    {
        private static readonly SemaphoreSlim _slimlock = new(1, 1);
        private static readonly IMemoryCache _memoryCache = ServiceLocator.Provider?.GetRequiredService<IMemoryCache>() ?? throw new NullReferenceException(nameof(IMemoryCache));
        private static readonly ILogger<DiscoverProvider> _logger = ServiceLocator.Provider?.GetRequiredService<ILogger<DiscoverProvider>>() ?? throw new NullReferenceException(nameof(ILogger));
        private readonly ConsulClient _consulClient = default!;

        internal DiscoverProvider(ConsulClient client)
        {
            _consulClient = client;
        }

        internal string ServiceName { get; set; } = string.Empty;
        internal ILoadBalancer LoadBalancer { get; set; } = default!;
        internal uint CacheSeconds { get; set; } = 5;

        public async Task<IList<string>> GetAllHealthServicesAsync()
        {
            var serviceAddressCacheKey = $"service_consul_{ServiceName}";
            var healthAddresses = _memoryCache.Get<List<string>>(serviceAddressCacheKey);
            if (healthAddresses.IsNotNullOrEmpty())
                return healthAddresses;

            await _slimlock.WaitAsync();
            try
            {
                _logger.LogDebug($"SemaphoreSlim=true,{serviceAddressCacheKey}");
                healthAddresses = _memoryCache.Get<List<string>>(serviceAddressCacheKey);
                if (healthAddresses.IsNotNullOrEmpty())
                    return healthAddresses;

                var query = await _consulClient.Health.Service(ServiceName, string.Empty, true);
                if (query is not null && query.Response.IsNotNullOrEmpty())
                {
                    var entryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(CacheSeconds)
                    };
                    healthAddresses = query.Response.Select(entry => $"{entry.Service.Address}:{entry.Service.Port}").ToList();
                    _memoryCache.Set(serviceAddressCacheKey, healthAddresses, entryOptions);
                }
                return healthAddresses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                _slimlock.Release();
            }
        }

        public async Task<string> GetSingleHealthServiceAsync()
        {
            var serviceList = await GetAllHealthServicesAsync();
            var service = LoadBalancer.Resolve(serviceList);
            return service;
        }
    }
}
