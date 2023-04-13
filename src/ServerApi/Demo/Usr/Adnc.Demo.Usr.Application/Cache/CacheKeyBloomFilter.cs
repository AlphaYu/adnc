namespace Adnc.Demo.Usr.Application.Cache;

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
            var values = new List<string>()
            {
                CachingConsts.MenuListCacheKey,
                CachingConsts.MenuTreeListCacheKey,
                CachingConsts.MenuRelationCacheKey,
                CachingConsts.MenuCodesCacheKey,
                CachingConsts.DetpListCacheKey,
                CachingConsts.DetpTreeListCacheKey,
                CachingConsts.DetpSimpleTreeListCacheKey,
                CachingConsts.RoleListCacheKey
            };

            using var scope = _serviceProvider.Value.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<User>>();
            var ids = await repository
                                                    .GetAll()
                                                    .Select(x => x.Id)
                                                    .ToListAsync();
            if (ids.IsNotNullOrEmpty())
                values.AddRange(ids.Select(x => string.Concat(CachingConsts.UserValidatedInfoKeyPrefix, GeneralConsts.LinkChar, x)));

            await InitAsync(values);
        }
    }
}