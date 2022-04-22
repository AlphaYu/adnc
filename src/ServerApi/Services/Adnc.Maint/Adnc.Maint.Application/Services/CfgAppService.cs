namespace Adnc.Maint.Application.Services;

public class CfgAppService : AbstractAppService, ICfgAppService
{
    private readonly IEfRepository<SysCfg> _cfgRepository;
    private readonly CacheService _cacheService;

    public CfgAppService(IEfRepository<SysCfg> cfgRepository
        , CacheService cacheService)
    {
        _cfgRepository = cfgRepository;
        _cacheService = cacheService;
    }

    public async Task<AppSrvResult<long>> CreateAsync(CfgCreationDto input)
    {
        var exist = (await _cacheService.GetAllCfgsFromCacheAsync()).Exists(c => c.Name.EqualsIgnoreCase(input.Name));
        if (exist)
            return Problem(HttpStatusCode.BadRequest, "参数名称已经存在");

        var cfg = Mapper.Map<SysCfg>(input);

        cfg.Id = IdGenerater.GetNextId();
        await _cfgRepository.InsertAsync(cfg);

        return cfg.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, CfgUpdationDto input)
    {
        var exist = (await _cacheService.GetAllCfgsFromCacheAsync()).Exists(c => c.Name.EqualsIgnoreCase(input.Name) && c.Id != id);
        if (exist)
            return Problem(HttpStatusCode.BadRequest, "参数名称已经存在");

        var entity = Mapper.Map<SysCfg>(input);

        entity.Id = id;
        var updatingProps = UpdatingProps<SysCfg>(x => x.Name, x => x.Value, x => x.Description);
        await _cfgRepository.UpdateAsync(entity, updatingProps);

        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long id)
    {
        await _cfgRepository.DeleteAsync(id);
        return AppSrvResult();
    }

    public async Task<CfgDto> GetAsync(long id)
    {
        return (await _cacheService.GetAllCfgsFromCacheAsync()).FirstOrDefault(x => x.Id == id);
    }

    public async Task<PageModelDto<CfgDto>> GetPagedAsync(CfgSearchPagedDto search)
    {
        var whereCondition = ExpressionCreator
                                            .New<CfgDto>()
                                            .AndIf(search.Name.IsNotNullOrWhiteSpace(), x => x.Name.Contains(search.Name))
                                            .AndIf(search.Value.IsNotNullOrWhiteSpace(), x => x.Value.Contains(search.Value));

        var allCfgs = await _cacheService.GetAllCfgsFromCacheAsync();
        var pagedCfgs = allCfgs.Where(whereCondition.Compile())
                                   .OrderByDescending(x => x.CreateTime)
                                   .Skip(search.SkipRows())
                                   .Take(search.PageSize)
                                   .ToArray();
        return new PageModelDto<CfgDto>(search, pagedCfgs, allCfgs.Count);
    }
}