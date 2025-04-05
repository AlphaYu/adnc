namespace Adnc.Demo.Admin.Application.Services;

public class SysConfigService(IEfRepository<SysConfig> sysConfigRepo/*, BloomFilterFactory bloomFilterFactory*/, CacheService cacheService)
    : AbstractAppService, ISysConfigService
{
    public async Task<ServiceResult<IdDto>> CreateAsync(SysConfigCreationDto input)
    {
        input.TrimStringFields();
        var keyExists = await sysConfigRepo.AnyAsync(x => x.Key == input.Key);
        if (keyExists)
        {
            return Problem(HttpStatusCode.BadRequest, "配置键已经存在");
        }

        var nameExists = await sysConfigRepo.AnyAsync(x => x.Name == input.Name);
        if (nameExists)
        {
            return Problem(HttpStatusCode.BadRequest, "配置名已经存在");
        }

        var entity = Mapper.Map<SysConfig>(input, IdGenerater.GetNextId());

        //var cacheKey = cacheService.ConcatCacheKey(CachingConsts.SysConfigSingleKeyPrefix, cfg.Id);
        //var cahceBf = bloomFilterFactory.Create(CachingConsts.BloomfilterOfCacheKey);
        //var addedStatus = await cahceBf.AddAsync(cacheKey);
        //if (!addedStatus)
        //    return Problem(HttpStatusCode.BadRequest, "添加到布隆过滤器失败!");
        //else
        //    await sysConfigRepo.InsertAsync(cfg);

        await sysConfigRepo.InsertAsync(entity);

        return new IdDto(entity.Id);
    }

    public async Task<ServiceResult> UpdateAsync(long id, SysConfigUpdationDto input)
    {
        input.TrimStringFields();

        var entity = await sysConfigRepo.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
        {
            return Problem(HttpStatusCode.NotFound, "配置不存在");
        }

        var keyExists = await sysConfigRepo.AnyAsync(c => c.Key == input.Key && c.Id != id);
        if (keyExists)
        {
            return Problem(HttpStatusCode.BadRequest, "配置键已经存在");
        }

        var nameExists = await sysConfigRepo.AnyAsync(c => c.Name == input.Name && c.Id != id);
        if (nameExists)
        {
            return Problem(HttpStatusCode.BadRequest, "配置名称已经存在");
        }

        var newEntity = Mapper.Map(input, entity);
        await sysConfigRepo.UpdateAsync(newEntity);

        return ServiceResult();
    }

    public async Task<ServiceResult> DeleteAsync(long[] ids)
    {
        await sysConfigRepo.ExecuteDeleteAsync(x => ids.Contains(x.Id));
        return ServiceResult();
    }

    public async Task<SysConfigDto?> GetAsync(long id)
    {
        var entity = await sysConfigRepo.FetchAsync(x => x.Id == id);
        return entity is null ? null : Mapper.Map<SysConfigDto>(entity);
    }

    public async Task<PageModelDto<SysConfigDto>> GetPagedAsync(SearchPagedDto input)
    {
        input.TrimStringFields();
        var whereExpr = ExpressionCreator
            .New<SysConfig>()
            .AndIf(input.Keywords.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Name, $"{input.Keywords}%") || EF.Functions.Like(x.Key, $"{input.Keywords}%"));

        var total = await sysConfigRepo.CountAsync(whereExpr);
        if (total == 0)
        {
            return new PageModelDto<SysConfigDto>(input);
        }

        var entities = await sysConfigRepo
                                        .Where(whereExpr)
                                        .OrderByDescending(x => x.Id)
                                        .Skip(input.SkipRows())
                                        .Take(input.PageSize)
                                        .ToListAsync();
        var cfgDtos = Mapper.Map<List<SysConfigDto>>(entities);

        return new PageModelDto<SysConfigDto>(input, cfgDtos, total);
    }

    public async Task<List<SysConfigSimpleDto>> GetListAsync(string keys)
    {
        if (keys.IsNullOrWhiteSpace())
        {
            return [];
        }

        var whereExpr = ExpressionCreator
           .New<SysConfigSimpleDto>()
           .AndIf(keys != "all", x => keys.Split(",", StringSplitOptions.RemoveEmptyEntries).Contains(x.Key));

        var result = (await cacheService.GetAllSysConfigsFromCacheAsync()).Where(whereExpr.Compile()).ToList();
        return result ?? [];
    }
}
