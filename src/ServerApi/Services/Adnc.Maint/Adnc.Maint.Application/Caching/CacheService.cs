namespace Adnc.Maint.Application.Services.Caching;

public class CacheService : AbstractCacheService
{
    private readonly Lazy<ICacheProvider> _cache;
    private readonly Lazy<IEfRepository<SysCfg>> _cfgRepository;
    private readonly Lazy<IEfRepository<SysDict>> _dictRepository;

    public CacheService(Lazy<ICacheProvider> cache
        , Lazy<IEfRepository<SysCfg>> cfgRepository
        , Lazy<IEfRepository<SysDict>> dictRepository)
        : base(cache)
    {
        _cache = cache;
        _cfgRepository = cfgRepository;
        _dictRepository = dictRepository;
    }

    public override async Task PreheatAsync()
    {
        await GetAllCfgsFromCacheAsync();
        await GetAllDictsFromCacheAsync();
    }

    public async Task<List<CfgDto>> GetAllCfgsFromCacheAsync()
    {
        var cahceValue = await _cache.Value.GetAsync(CachingConsts.CfgListCacheKey, async () =>
        {
            var allCfgs = await _cfgRepository.Value.GetAll(writeDb: true).ToListAsync();
            return Mapper.Map<List<CfgDto>>(allCfgs);
        }, TimeSpan.FromSeconds(CachingConsts.OneYear));

        return cahceValue.Value;
    }

    public async Task<List<DictDto>> GetAllDictsFromCacheAsync()
    {
        var cahceValue = await _cache.Value.GetAsync(CachingConsts.DictListCacheKey, async () =>
        {
            var allDicts = await _dictRepository.Value.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Map<List<DictDto>>(allDicts);
        }, TimeSpan.FromSeconds(CachingConsts.OneYear));

        return cahceValue.Value;
    }
}