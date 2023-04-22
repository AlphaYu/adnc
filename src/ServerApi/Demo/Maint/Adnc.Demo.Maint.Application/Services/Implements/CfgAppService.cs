namespace Adnc.Demo.Maint.Application.Services.Implements;

public class CfgAppService : AbstractAppService, ICfgAppService
{
    private readonly IEfRepository<Cfg> _cfgRepository;
    private readonly BloomFilterFactory _bloomFilterFactory;
    private readonly CacheService _cacheService;

    public CfgAppService(
        IEfRepository<Cfg> cfgRepository,
        BloomFilterFactory bloomFilterFactory,
        CacheService cacheService)
    {
        _cfgRepository = cfgRepository;
        _bloomFilterFactory = bloomFilterFactory;
        _cacheService = cacheService;
    }

    public async Task<AppSrvResult<long>> CreateAsync(CfgCreationDto input)
    {
        input.TrimStringFields();
        var exists = await _cfgRepository.AnyAsync(x => x.Name.Equals(input.Name));
        if (exists)
            return Problem(HttpStatusCode.BadRequest, "参数名称已经存在");

        var cfg = Mapper.Map<Cfg>(input);
        cfg.Id = IdGenerater.GetNextId();

        var cacheKey = _cacheService.ConcatCacheKey(CachingConsts.CfgSingleKeyPrefix, cfg.Id);
        var cahceBf = _bloomFilterFactory.Create(CachingConsts.BloomfilterOfCacheKey);
        var addedStatus = await cahceBf.AddAsync(cacheKey);
        if (!addedStatus)
            return Problem(HttpStatusCode.BadRequest, "添加到布隆过滤器失败!");
        else
            await _cfgRepository.InsertAsync(cfg);

        return cfg.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, CfgUpdationDto input)
    {
        input.TrimStringFields();
        var exists = await _cfgRepository.AnyAsync(c => c.Name.Equals(input.Name) && c.Id != id);
        if (exists)
            return Problem(HttpStatusCode.BadRequest, "参数名称已经存在");

        var entity = Mapper.Map<Cfg>(input);

        entity.Id = id;
        var updatingProps = UpdatingProps<Cfg>(x => x.Name, x => x.Value, x => x.Description);
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
        var entity = await _cfgRepository.FindAsync(id);
        return Mapper.Map<CfgDto>(entity);
    }

    public async Task<PageModelDto<CfgDto>> GetPagedAsync(CfgSearchPagedDto search)
    {
        search.TrimStringFields();
        var whereExpression = ExpressionCreator
            .New<Cfg>()
            .AndIf(search.Name.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Name, $"{search.Name}%"))
            .AndIf(search.Value.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Value, $"{search.Value}%"));


        var total = await _cfgRepository.CountAsync(whereExpression);
        if (total == 0)
            return new PageModelDto<CfgDto>(search);

        var entities = await _cfgRepository
                                        .Where(whereExpression)
                                        .OrderByDescending(x => x.ModifyTime)
                                        .Skip(search.SkipRows())
                                        .Take(search.PageSize)
                                        .ToListAsync();
        var cfgDtos = Mapper.Map<List<CfgDto>>(entities);

        return new PageModelDto<CfgDto>(search, cfgDtos, total);
    }
}