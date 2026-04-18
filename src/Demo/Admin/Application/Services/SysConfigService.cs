using Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig;

namespace Adnc.Demo.Admin.Application.Services;

/// <inheritdoc cref="ISysConfigService"/>
public class SysConfigService(IEfRepository<SysConfig> sysConfigRepo/*, BloomFilterFactory bloomFilterFactory*/, CacheService cacheService)
    : AbstractAppService, ISysConfigService
{
    /// <inheritdoc />
    public async Task<ServiceResult<IdDto>> CreateAsync(SysConfigCreationDto input)
    {
        input.TrimStringFields();
        var keyExists = await sysConfigRepo.AnyAsync(x => x.Key == input.Key);
        if (keyExists)
        {
            return Problem(HttpStatusCode.BadRequest, "This configuration key already exists");
        }

        var nameExists = await sysConfigRepo.AnyAsync(x => x.Name == input.Name);
        if (nameExists)
        {
            return Problem(HttpStatusCode.BadRequest, "This configuration name already exists");
        }

        var entity = Mapper.Map<SysConfig>(input, IdGenerater.GetNextId());

        //var cacheKey = cacheService.ConcatCacheKey(CachingConsts.SysConfigSingleKeyPrefix, cfg.Id);
        //var cahceBf = bloomFilterFactory.Create(CachingConsts.BloomfilterOfCacheKey);
        //var addedStatus = await cahceBf.AddAsync(cacheKey);
        //if (!addedStatus)
        //    return Problem(HttpStatusCode.BadRequest, "Failed to add the item to the bloom filter.");
        //else
        //    await sysConfigRepo.InsertAsync(cfg);

        await sysConfigRepo.InsertAsync(entity);

        return new IdDto(entity.Id);
    }

    /// <inheritdoc />
    public async Task<ServiceResult> UpdateAsync(long id, SysConfigUpdationDto input)
    {
        input.TrimStringFields();

        var entity = await sysConfigRepo.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
        {
            return Problem(HttpStatusCode.NotFound, "This configuration does not exist");
        }

        var keyExists = await sysConfigRepo.AnyAsync(c => c.Key == input.Key && c.Id != id);
        if (keyExists)
        {
            return Problem(HttpStatusCode.BadRequest, "This configuration key already exists");
        }

        var nameExists = await sysConfigRepo.AnyAsync(c => c.Name == input.Name && c.Id != id);
        if (nameExists)
        {
            return Problem(HttpStatusCode.BadRequest, "This configuration name already exists");
        }

        var newEntity = Mapper.Map(input, entity);
        await sysConfigRepo.UpdateAsync(newEntity);

        return ServiceResult();
    }

    /// <inheritdoc />
    public async Task<ServiceResult> DeleteAsync(long[] ids)
    {
        await sysConfigRepo.ExecuteDeleteAsync(x => ids.Contains(x.Id));
        return ServiceResult();
    }

    /// <inheritdoc />
    public async Task<SysConfigDto?> GetAsync(long id)
    {
        var entity = await sysConfigRepo.FetchAsync(x => x.Id == id);
        return entity is null ? null : Mapper.Map<SysConfigDto>(entity);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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
