namespace Adnc.Cus.Application.BloomFilters;

public class CacheKeyBloomFilter : AbstractBloomFilter
{
    private readonly Lazy<IServiceProvider> _services;
    private readonly CacheOptions.BloomFilterSetting _setting;

    public CacheKeyBloomFilter(Lazy<ICacheProvider> cache
        , Lazy<IRedisProvider> redisProvider
        , Lazy<IDistributedLocker> distributedLocker
        , Lazy<IServiceProvider> services)
        : base(redisProvider, distributedLocker)
    {
        _services = services;
        _setting = cache.Value.CacheOptions.PenetrationSetting.BloomFilterSetting;
    }

    public override string Name => _setting.Name;

    public override double ErrorRate => _setting.ErrorRate;

    public override int Capacity => _setting.Capacity;

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