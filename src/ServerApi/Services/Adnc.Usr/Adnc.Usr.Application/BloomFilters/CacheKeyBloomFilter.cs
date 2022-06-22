namespace Adnc.Usr.Application.BloomFilters;

public class CacheKeyBloomFilter : AbstractBloomFilter
{
    private readonly Lazy<IServiceProvider> _serviceProvider;
    private readonly CacheOptions.BloomFilterSetting _setting;

    public CacheKeyBloomFilter(Lazy<ICacheProvider> cache
        , Lazy<IRedisProvider> redisProvider
        , Lazy<IDistributedLocker> distributedLocker
        , Lazy<IServiceProvider> serviceProvider)
        : base(redisProvider, distributedLocker)
    {
        _serviceProvider = serviceProvider;
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
            var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysUser>>();
            var ids = await repository
                                                    .GetAll()
                                                    .Select(x => x.Id)
                                                    .ToListAsync();
            if (ids.IsNotNullOrEmpty())
                values.AddRange(ids.Select(x => string.Concat(CachingConsts.UserValidatedInfoKeyPrefix, CachingConsts.LinkChar, x)));

            await InitAsync(values);
        }
    }
}