namespace Adnc.Demo.Admin.Application.Services;

public class SysConfigService(IEfRepository<SysConfig> sysConfigRepo, BloomFilterFactory bloomFilterFactory,CacheService cacheService) 
    : AbstractAppService, ISysConfigService
{

    public async Task<ServiceResult<long>> CreateAsync(SysConfigCreationDto input)
    {
        input.TrimStringFields();
        var keyExists = await sysConfigRepo.AnyAsync(x => x.Key == input.Key);
        if (keyExists)
            return Problem(HttpStatusCode.BadRequest, "配置键已经存在");

        var nameExists = await sysConfigRepo.AnyAsync(x => x.Name == input.Name);
        if (nameExists)
            return Problem(HttpStatusCode.BadRequest, "配置名已经存在");

        var entity = Mapper.Map<SysConfig>(input, IdGenerater.GetNextId());

        //var cacheKey = cacheService.ConcatCacheKey(CachingConsts.SysConfigSingleKeyPrefix, cfg.Id);
        //var cahceBf = bloomFilterFactory.Create(CachingConsts.BloomfilterOfCacheKey);
        //var addedStatus = await cahceBf.AddAsync(cacheKey);
        //if (!addedStatus)
        //    return Problem(HttpStatusCode.BadRequest, "添加到布隆过滤器失败!");
        //else
        //    await sysConfigRepo.InsertAsync(cfg);

        await sysConfigRepo.InsertAsync(entity);

        return entity.Id;
    }

    public async Task<ServiceResult> UpdateAsync(long id, SysConfigUpdationDto input)
    {
        input.TrimStringFields();

        var entity = await sysConfigRepo.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
            return Problem(HttpStatusCode.NotFound, "配置不存在");

        var keyExists = await sysConfigRepo.AnyAsync(c => c.Key == input.Key && c.Id != id);
        if (keyExists)
            return Problem(HttpStatusCode.BadRequest, "配置键已经存在");

        var nameExists = await sysConfigRepo.AnyAsync(c => c.Name == input.Name && c.Id != id);
        if (nameExists)
            return Problem(HttpStatusCode.BadRequest, "配置名称已经存在");

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

    public async Task<PageModelDto<SysConfigDto>> GetPagedAsync(SysConfigSearchPagedDto search)
    {
        search.TrimStringFields();
        var whereExpr = ExpressionCreator
            .New<SysConfig>()
            .AndIf(search.Keywords.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Name, $"{search.Keywords}%") || EF.Functions.Like(x.Key, $"{search.Keywords}%"));

        var total = await sysConfigRepo.CountAsync(whereExpr);
        if (total == 0)
            return new PageModelDto<SysConfigDto>(search);

        var entities = await sysConfigRepo
                                        .Where(whereExpr)
                                        .OrderByDescending(x => x.Id)
                                        .Skip(search.SkipRows())
                                        .Take(search.PageSize)
                                        .ToListAsync();
        var cfgDtos = Mapper.Map<List<SysConfigDto>>(entities);

        return new PageModelDto<SysConfigDto>(search, cfgDtos, total);
    }
}