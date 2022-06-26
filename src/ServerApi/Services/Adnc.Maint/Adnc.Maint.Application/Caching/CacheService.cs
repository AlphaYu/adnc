using Adnc.Infra.Core.Interfaces;

namespace Adnc.Maint.Application.Services.Caching;

public sealed class CacheService : AbstractCacheService, ICachePreheatable
{
    private Lazy<IDistributedLocker> _dictributeLocker;
    private ILogger<CacheService> _logger;

    public CacheService(
        Lazy<ICacheProvider> cacheProvider,
        Lazy<IDistributedLocker> dictributeLocker,
        Lazy<IServiceProvider> serviceProvider,
         ILogger<CacheService> logger)
        : base(cacheProvider, serviceProvider)
    {
        _dictributeLocker = dictributeLocker;
        _logger = logger;
    }

    public override async Task PreheatAsync()
    {
        await PreheatAllDictsAsync();
        await PreheatAllCfgsAsync();
    }

    private async Task PreheatAllDictsAsync()
    {
        var exists = await CacheProvider.Value.ExistsAsync(CachingConsts.DictPreheatedKey);
        if (exists)
            return;

        var flag = await _dictributeLocker.Value.LockAsync(CachingConsts.DictPreheatedKey);
        if (!flag.Success)
        {
            await Task.Delay(500);
            await PreheatAllDictsAsync();
        }

        using var scope = ServiceProvider.Value.CreateScope();
        var dictRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysDict>>();
        var dictEntities = await dictRepository.GetAll().ToListAsync();
        if (dictEntities.IsNullOrEmpty())
            return;

        var parentEntities = dictEntities.Where(x => x.Pid == 0).ToList();
        var childrenEntities = dictEntities.Where(x => x.Pid > 0).ToList();
        var parentDtos = Mapper.Value.Map<List<DictDto>>(parentEntities);
        var childrenDtos = Mapper.Value.Map<List<DictDto>>(childrenEntities);

        _logger.LogInformation($"start preheat {CachingConsts.DictSingleKeyPrefix}");
        var cahceDictionary = new Dictionary<string, DictDto>();
        for (var index = 1; index <= parentDtos.Count; index++)
        {
            var dto = parentDtos[index - 1];
            var subDtos = childrenDtos?.Where(x => x.Pid == dto.Id).ToList();
            if (subDtos.IsNotNullOrEmpty())
            {
                dto.Children = subDtos;
            }

            var cacheKey = ConcatCacheKey(CachingConsts.DictSingleKeyPrefix, dto.Id);
            cahceDictionary.Add(cacheKey, dto);
            if (index % 50 == 0 || index == parentDtos.Count)
            {
                await CacheProvider.Value.SetAllAsync(cahceDictionary, TimeSpan.FromSeconds(CachingConsts.OneMonth));
                cahceDictionary.Clear();
            }
        }

        var serverInfo = ServiceProvider.Value.GetService<IServiceInfo>();
        await CacheProvider.Value.SetAsync(CachingConsts.DictPreheatedKey, serverInfo.Version, TimeSpan.FromSeconds(CachingConsts.OneYear));
        _logger.LogInformation($"finished({parentDtos.Count}) preheat {CachingConsts.DictSingleKeyPrefix}");
    }

    private async Task PreheatAllCfgsAsync()
    {
        var exists = await CacheProvider.Value.ExistsAsync(CachingConsts.CfgPreheatedKey);
        if (exists)
            return;

        var flag = await _dictributeLocker.Value.LockAsync(CachingConsts.CfgPreheatedKey);
        if (!flag.Success)
        {
            await Task.Delay(500);
            await PreheatAllCfgsAsync();
        }

        using var scope = ServiceProvider.Value.CreateScope();
        var cfgRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysCfg>>();
        var cfgEntities = await cfgRepository.GetAll().ToListAsync();
        if (cfgEntities.IsNullOrEmpty())
            return;

        var cfgDtos = Mapper.Value.Map<List<CfgDto>>(cfgEntities);
        _logger.LogInformation($"start preheat {CachingConsts.CfgSingleKeyPrefix}");
        var cahceDictionary = new Dictionary<string, CfgDto>();
        for (var index = 1; index <= cfgDtos.Count; index++)
        {
            var dto = cfgDtos[index - 1];
            var cacheKey = ConcatCacheKey(CachingConsts.CfgSingleKeyPrefix, dto.Id);
            cahceDictionary.Add(cacheKey, dto);
            if (index % 50 == 0 || index == cfgDtos.Count)
            {
                await CacheProvider.Value.SetAllAsync(cahceDictionary, TimeSpan.FromSeconds(CachingConsts.OneMonth));
                cahceDictionary.Clear();
            }
        }

        var serverInfo = ServiceProvider.Value.GetService<IServiceInfo>();
        await CacheProvider.Value.SetAsync(CachingConsts.CfgPreheatedKey, serverInfo.Version, TimeSpan.FromSeconds(CachingConsts.OneYear));
        _logger.LogInformation($"finished({cfgDtos.Count}) preheat {CachingConsts.CfgSingleKeyPrefix}");
    }
}