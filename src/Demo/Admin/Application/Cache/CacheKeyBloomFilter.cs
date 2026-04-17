namespace Adnc.Demo.Admin.Application.Cache;

public class CacheKeyBloomFilter(Lazy<IOptions<CacheOptions>> cacheOptions, Lazy<IRedisProvider> redisProvider, Lazy<IDistributedLocker> distributedLocker/*, Lazy<IServiceProvider> serviceProvider*/)
    : AbstractBloomFilter(redisProvider, distributedLocker)
{
    public override string Name => cacheOptions.Value.Value.PenetrationSetting.BloomFilterSetting.Name;

    public override double ErrorRate => cacheOptions.Value.Value.PenetrationSetting.BloomFilterSetting.ErrorRate;

    public override int Capacity => cacheOptions.Value.Value.PenetrationSetting.BloomFilterSetting.Capacity;

    public override async Task InitAsync()
    {
        await Task.CompletedTask;
        //var exists = await ExistsBloomFilterAsync();
        //if (!exists)
        //{
        //    var values = new List<string>()
        //    {
        //        CachingConsts.MenuListCacheKey,
        //        CachingConsts.RoleMenuCodesCacheKey,
        //        CachingConsts.DetpListCacheKey
        //    };

        //    using var scope = serviceProvider.Value.CreateScope();
        //    var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<User>>();
        //    var ids = await repository
        //                                            .GetAll()
        //                                            .Select(x => x.Id)
        //                                            .ToListAsync();
        //    if (ids.IsNotNullOrEmpty())
        //        values.AddRange(ids.Select(x => string.Concat(CachingConsts.UserValidatedInfoKeyPrefix, GeneralConsts.LinkChar, x)));

        //    await InitAsync(values);
        //}
    }
}
