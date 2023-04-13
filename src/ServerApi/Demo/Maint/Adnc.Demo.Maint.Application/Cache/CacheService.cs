namespace Adnc.Demo.Maint.Application.Cache;

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

        var locker = await _dictributeLocker.Value.LockAsync(CachingConsts.DictPreheatedKey);
        if (!locker.Success)
        {
            await Task.Delay(500);
            await PreheatAllDictsAsync();
        }

        try
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var dictRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<Dict>>();
            var dictEntities = await dictRepository.GetAll().ToListAsync();
            if (dictEntities.IsNullOrEmpty())
                return;

            var parentEntities = dictEntities.Where(x => x.Pid == 0).ToList();
            var childrenEntities = dictEntities.Where(x => x.Pid > 0).ToList();
            var parentDtos = Mapper.Value.Map<List<DictDto>>(parentEntities);
            var childrenDtos = Mapper.Value.Map<List<DictDto>>(childrenEntities);
            var cahceDictionary = new Dictionary<string, DictDto>();

            _logger.LogInformation($"start preheat {CachingConsts.DictSingleKeyPrefix}");
            for (var index = 1; index <= parentDtos.Count; index++)
            {
                var dto = parentDtos[index - 1];
                var subDtos = childrenDtos?.Where(x => x.Pid == dto.Id).ToList();
                if (subDtos is not null && subDtos.Count > 0)
                {
                    dto.Children = subDtos;
                }
                var cacheKey = ConcatCacheKey(CachingConsts.DictSingleKeyPrefix, dto.Id);
                cahceDictionary.Add(cacheKey, dto);
                if (index % 50 == 0 || index == parentDtos.Count)
                {
                    await CacheProvider.Value.SetAllAsync(cahceDictionary, TimeSpan.FromSeconds(GeneralConsts.OneMonth));
                    cahceDictionary.Clear();
                }
            }
            _logger.LogInformation($"finished({parentDtos.Count}) preheat {CachingConsts.DictSingleKeyPrefix}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await _dictributeLocker.Value.SafedUnLockAsync(CachingConsts.DictPreheatedKey, locker.LockValue);
            throw new Exception("PreheatAllCfgsAsync was failure", ex);
        }
        var serverInfo = ServiceProvider.Value.GetRequiredService<IServiceInfo>();
        await CacheProvider.Value.SetAsync(CachingConsts.DictPreheatedKey, serverInfo.Version, TimeSpan.FromSeconds(GeneralConsts.OneYear));
    }

    private async Task PreheatAllCfgsAsync()
    {
        var exists = await CacheProvider.Value.ExistsAsync(CachingConsts.CfgPreheatedKey);
        if (exists)
            return;

        var locker = await _dictributeLocker.Value.LockAsync(CachingConsts.CfgPreheatedKey);
        if (!locker.Success)
        {
            await Task.Delay(500);
            await PreheatAllCfgsAsync();
        }

        try
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var cfgRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<Cfg>>();
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
                    await CacheProvider.Value.SetAllAsync(cahceDictionary, TimeSpan.FromSeconds(GeneralConsts.OneMonth));
                    cahceDictionary.Clear();
                }
            }
            _logger.LogInformation($"finished({cfgDtos.Count}) preheat {CachingConsts.CfgSingleKeyPrefix}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await _dictributeLocker.Value.SafedUnLockAsync(CachingConsts.DictPreheatedKey, locker.LockValue);
            throw new Exception("PreheatAllCfgsAsync was failure", ex);
        }
        var serverInfo = ServiceProvider.Value.GetRequiredService<IServiceInfo>();
        await CacheProvider.Value.SetAsync(CachingConsts.CfgPreheatedKey, serverInfo.Version, TimeSpan.FromSeconds(GeneralConsts.OneYear));
    }
}