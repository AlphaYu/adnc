using Microsoft.Extensions.Caching.Memory;

namespace Adnc.Infra.Consul.Discover
{
    public class ConsulServiceProvider : IConsulServiceProvider
    {
        private static readonly SemaphoreSlim _slimlock = new(1, 1);
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ConsulServiceProvider> _logger;

        private readonly ConsulClient _consulClient;

        public ConsulServiceProvider(ConsulClient consulClient
            , IMemoryCache memoryCache
            , ILogger<ConsulServiceProvider> logger)
        {
            if (consulClient is null)
                throw new ArgumentNullException(nameof(consulClient));

            _consulClient = consulClient;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<IList<ServiceEntry>> GetAllServicesAsync(string serviceName)
        {
            var queryResult = await _consulClient.Health.Service(serviceName, string.Empty, false);
            return queryResult.Response;
        }

        public async Task<IList<string>> GetHealthServicesAsync(string serviceName)
        {
            var serviceAddressCacheKey = $"service_consul_{serviceName}";
            var healthAddresses = _memoryCache.Get<List<string>>(serviceAddressCacheKey);
            if (healthAddresses.IsNotNullOrEmpty())
                return healthAddresses;

            await _slimlock.WaitAsync();
            try
            {
                _logger.LogInformation($"SemaphoreSlim=true,{serviceAddressCacheKey}");
                healthAddresses = _memoryCache.Get<List<string>>(serviceAddressCacheKey);
                if (healthAddresses.IsNotNullOrEmpty())
                    return healthAddresses;

                var query = await _consulClient.Health.Service(serviceName, string.Empty, true);
                if (query is not null && query.Response.IsNotNullOrEmpty())
                {
                    var entryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3)
                    };
                    healthAddresses = query.Response.Select(entry => $"{entry.Service.Address}:{entry.Service.Port}").ToList();
                    _memoryCache.Set(serviceAddressCacheKey, healthAddresses, entryOptions);
                }
                return healthAddresses;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                _slimlock.Release();
            }
        }
    }

    public interface IConsulServiceProvider
    {
        Task<IList<string>> GetHealthServicesAsync(string serviceName);
    }
}