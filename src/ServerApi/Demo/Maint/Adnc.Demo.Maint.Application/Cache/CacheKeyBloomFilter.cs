namespace Adnc.Demo.Maint.Application.Cache;

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
            var values = new List<string>();
            using var scope = _serviceProvider.Value.CreateScope();
            var dictRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<Dict>>();
            var dictIds = await dictRepository
                .Where(x => x.Pid == 0)
                .Select(x => x.Id)
                .ToListAsync();
            if (dictIds.IsNotNullOrEmpty())
                values.AddRange(dictIds.Select(x => string.Concat(CachingConsts.DictSingleKeyPrefix, GeneralConsts.LinkChar, x)));

            var cfgRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<Dict>>();
            var cfgIds = await dictRepository
                .GetAll()
                .Select(x => x.Id)
                .ToListAsync();
            if (cfgIds.IsNotNullOrEmpty())
                values.AddRange(cfgIds.Select(x => string.Concat(CachingConsts.CfgSingleKeyPrefix, GeneralConsts.LinkChar, x)));

            await InitAsync(values);
        }
    }
}