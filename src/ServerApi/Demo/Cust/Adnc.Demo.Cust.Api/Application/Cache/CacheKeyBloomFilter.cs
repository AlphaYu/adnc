namespace Adnc.Demo.Cust.Api.Application.Cache;

public class CacheKeyBloomFilter : AbstractBloomFilter
{
    private readonly Lazy<IServiceProvider> _serviceProvider;
    private readonly Lazy<IOptions<CacheOptions>> _cacheOptions;

    public CacheKeyBloomFilter(
        Lazy<IOptions<CacheOptions>> cacheOptions,
        Lazy<IRedisProvider> redisProvider,
        Lazy<IDistributedLocker> distributedLocker,
        Lazy<IServiceProvider> serviceProvider)
        : base(redisProvider, distributedLocker)
    {
        _serviceProvider = serviceProvider;
        _cacheOptions = cacheOptions;
    }

    public override string Name => _cacheOptions.Value.Value.PenetrationSetting.BloomFilterSetting.Name;

    public override double ErrorRate => _cacheOptions.Value.Value.PenetrationSetting.BloomFilterSetting.ErrorRate;

    public override int Capacity => _cacheOptions.Value.Value.PenetrationSetting.BloomFilterSetting.Capacity;

    public override async Task InitAsync()
    {
        var exists = await ExistsBloomFilterAsync();
        if (!exists)
        {
            // // TODO: init values.
        }
        await Task.CompletedTask;
    }
}