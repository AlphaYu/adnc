namespace Adnc.Maint.Application.Services.Caching;

public sealed class CacheService : AbstractCacheService, ICachePreheatable
{
    public CacheService(Lazy<ICacheProvider> cacheProvider, Lazy<IServiceProvider> serviceProvider)
        : base(cacheProvider, serviceProvider)
    {
    }

    public override async Task PreheatAsync()
    {
        await GetAllCfgsFromCacheAsync();
        await GetAllDictsFromCacheAsync();
    }

    public async Task<List<CfgDto>> GetAllCfgsFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.CfgListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var cfgRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysCfg>>();
            var allCfgs = await cfgRepository.GetAll(writeDb: true).ToListAsync();
            return Mapper.Value.Map<List<CfgDto>>(allCfgs);
        }, TimeSpan.FromSeconds(CachingConsts.OneYear));

        return cahceValue.Value;
    }

    public async Task<List<DictDto>> GetAllDictsFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.DictListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var dictRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysDict>>();
            var allDicts = await dictRepository.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<DictDto>>(allDicts);
        }, TimeSpan.FromSeconds(CachingConsts.OneYear));

        return cahceValue.Value;
    }
}