namespace Adnc.Maint.Application.Services;

public class DictAppService : AbstractAppService, IDictAppService
{
    private readonly IEfRepository<SysDict> _dictRepository;
    private readonly BloomFilterFactory _bloomFilterFactory;
    private readonly CacheService _cacheService;

    public DictAppService(
        IEfRepository<SysDict> dictRepository,
        BloomFilterFactory bloomFilterFactory,
        CacheService cacheService)
    {
        _dictRepository = dictRepository;
        _bloomFilterFactory = bloomFilterFactory;
        _cacheService = cacheService;
    }

    public async Task<AppSrvResult<long>> CreateAsync(DictCreationDto input)
    {
        var exists = await _dictRepository.AnyAsync(x => x.Name.Equals(input.Name.Trim()));
        if (exists)
            return Problem(HttpStatusCode.BadRequest, "字典名字已经存在");

        var dists = new List<SysDict>();
        long id = IdGenerater.GetNextId();
        var dict = new SysDict { Id = id, Name = input.Name, Value = input.Value, Ordinal = input.Ordinal, Pid = 0 };

        dists.Add(dict);
        input.Children?.ForEach(x =>
        {
            dists.Add(new SysDict
            {
                Id = IdGenerater.GetNextId(),
                Pid = id,
                Name = x.Name,
                Value = x.Value,
                Ordinal = x.Ordinal
            });
        });

        var cacheKey = _cacheService.ConcatCacheKey(CachingConsts.DictSingleKeyPrefix, id);
        var cahceBf = _bloomFilterFactory.Create(CachingConsts.BloomfilterOfCacheKey);
        var addedStatus = await cahceBf.AddAsync(cacheKey);
        if (!addedStatus)
            return Problem(HttpStatusCode.BadRequest, "添加到布隆过滤器失败!");
        else
            await _dictRepository.InsertRangeAsync(dists);
        return id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, DictUpdationDto input)
    {
        var exists = await _dictRepository.AnyAsync(x => x.Name.Equals(input.Name.Trim()) && x.Id != id);
        if (exists)
            return Problem(HttpStatusCode.BadRequest, "字典名字已经存在");

        var dict = new SysDict { Name = input.Name, Value = input.Value, Id = id, Pid = 0, Ordinal = input.Ordinal };

        var subDicts = new List<SysDict>();
        input.Children?.ForEach(x =>
        {
            subDicts.Add(new SysDict
            {
                Id = IdGenerater.GetNextId(),
                Pid = id,
                Name = x.Name,
                Value = x.Value,
                Ordinal = x.Ordinal
            });
        });

        await _dictRepository.UpdateAsync(dict, UpdatingProps<SysDict>(d => d.Name, d => d.Value, d => d.Ordinal));
        await _dictRepository.DeleteRangeAsync(d => d.Pid == dict.Id);
        if (subDicts.IsNotNullOrEmpty())
            await _dictRepository.InsertRangeAsync(subDicts);

        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long id)
    {
        await _dictRepository.DeleteRangeAsync(d => (d.Id == id) || (d.Pid == id));
        return AppSrvResult();
    }

    public async Task<DictDto> GetAsync(long id)
    {
        var dictEntity = await _dictRepository.FindAsync(id);
        if (dictEntity is null)
            return default;

        var dictDto = Mapper.Map<DictDto>(dictEntity);
        var subDictEnties = await _dictRepository.Where(x => x.Pid == id).ToListAsync();
        if (subDictEnties is not null)
        {
            var subDictDtos = Mapper.Map<List<DictDto>>(subDictEnties);
            dictDto.Children = subDictDtos;
        }
        return dictDto;
    }

    public async Task<List<DictDto>> GetListAsync(DictSearchDto search)
    {
        var result = new List<DictDto>();

        var whereCondition = ExpressionCreator
            .New<SysDict>(x => x.Pid == 0)
            .AndIf(search.Name.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Name, $"{search.Name.Trim()}%"));

        var dictEntities = await _dictRepository
            .Where(whereCondition)
            .OrderBy(d => d.Ordinal)
            .ToListAsync();

        if (dictEntities is null)
            return new List<DictDto>();

        var subPids = dictEntities.Select(x => x.Id);
        var allSubDictEntities =await _dictRepository.Where(x => subPids.Contains(x.Pid)).ToListAsync();

        var dictDtos = Mapper.Map<List<DictDto>>(dictEntities);
        var allSubDictDtos = Mapper.Map<List<DictDto>>(allSubDictEntities);
        foreach (var dto in dictDtos)
        {
            var subDtos = allSubDictDtos?.Where(x => x.Pid == dto.Id).ToList();
            if (subDtos.IsNotNullOrEmpty())
            {
                dto.Children = subDtos;
            }
        }

        return dictDtos;
    }
}