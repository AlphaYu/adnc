﻿namespace Adnc.Demo.Admin.Application.Services;

public class DictService(IEfRepository<Dict> dictRepo, IEfRepository<DictData> dictDataRepo, BloomFilterFactory bloomFilterFactory, CacheService cacheService)
    : AbstractAppService, IDictService
{
    public async Task<ServiceResult<long>> CreateAsync(DictCreationDto input)
    {
        input.TrimStringFields();

        var codeExists = await dictRepo.AnyAsync(x => x.Code == input.Code);
        if (codeExists)
            return Problem(HttpStatusCode.BadRequest, "字典编码已经存在");

        var nameExists = await dictRepo.AnyAsync(x => x.Name == input.Name);
        if (nameExists)
            return Problem(HttpStatusCode.BadRequest, "字典名字已经存在");

        var entity = Mapper.Map<Dict>(input, IdGenerater.GetNextId());

        //var cacheKey = cacheService.ConcatCacheKey(CachingConsts.DictOptionSingleKeyPrefix, id);
        //var cahceBf = bloomFilterFactory.Create(CachingConsts.BloomfilterOfCacheKey);
        //var addedStatus = await cahceBf.AddAsync(cacheKey);
        //if (!addedStatus)
        //    return Problem(HttpStatusCode.BadRequest, "添加到布隆过滤器失败!");
        //else
        //    await dictRepo.InsertRangeAsync(dists);

        await dictRepo.InsertAsync(entity);
        return entity.Id;
    }

    public async Task<ServiceResult> UpdateAsync(long id, DictUpdationDto input)
    {
        var entity = await dictRepo.FetchAsync(x => x.Id == id, noTracking: false);
        if (entity is null)
            return Problem(HttpStatusCode.NotFound, "字典不存在");

        input.TrimStringFields();
        var codeExists = await dictRepo.AnyAsync(x => x.Code == input.Code && x.Id != id);
        if (codeExists)
            return Problem(HttpStatusCode.BadRequest, "字典编码已经存在");

        var nameExists = await dictRepo.AnyAsync(x => x.Name == input.Name && x.Id != id);
        if (nameExists)
            return Problem(HttpStatusCode.BadRequest, "字典名字已经存在");

        if (input.Code != entity.Code)
            await dictDataRepo.ExecuteUpdateAsync(x => x.DictCode == input.Code, setters => setters.SetProperty(x => x.DictCode, input.Code));

        var newEntity = Mapper.Map(input, entity);
        await dictRepo.UpdateAsync(newEntity);

        return ServiceResult();
    }

    public async Task<ServiceResult> DeleteAsync(long[] ids)
    {
        await dictRepo.ExecuteDeleteAsync(x => ids.Contains(x.Id));

        var dictCodes = await dictRepo.Where(x => ids.Contains(x.Id)).Select(x => x.Code).ToListAsync();
        if (dictCodes.IsNotNullOrEmpty())
            await dictDataRepo.ExecuteDeleteAsync(x => dictCodes.Contains(x.DictCode));

        return ServiceResult();
    }

    public async Task<DictDto?> GetAsync(long id)
    {
        var entity = await dictRepo.FetchAsync(x => x.Id == id);
        if (entity is null)
            return null;

        var dictDto = Mapper.Map<DictDto>(entity);
        return dictDto;
    }

    public async Task<PageModelDto<DictDto>> GetPagedAsync(DictSearchPagedDto search)
    {
        search.TrimStringFields();
        var whereExpr = ExpressionCreator
            .New<Dict>()
            .AndIf(search.Keywords.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Name, $"{search.Keywords}%") || EF.Functions.Like(x.Code, $"{search.Keywords}%"));

        var total = await dictRepo.CountAsync(whereExpr);
        if (total == 0)
            return new PageModelDto<DictDto>(search);

        var entities = await dictRepo
                                        .Where(whereExpr)
                                        .OrderByDescending(x => x.Id)
                                        .Skip(search.SkipRows())
                                        .Take(search.PageSize)
                                        .ToListAsync();
        var cfgDtos = Mapper.Map<List<DictDto>>(entities);

        return new PageModelDto<DictDto>(search, cfgDtos, total);
    }

    public async Task<List<DictOption>> GetOptionsAsync()
    {
        var options = await cacheService.GetAllDictOptionsFromCacheAsync();
        return options ?? [];
    }
}