namespace Adnc.Demo.Maint.Application.Services.Implements;

public class LogAppService : AbstractAppService, ILogAppService
{
    private readonly IMongoRepository<OperationLog> _opsLogRepository;
    private readonly IMongoRepository<LoggerLog> _nlogLogRepository;
    private readonly IMongoRepository<LoginLog> _loginLogRepository;

    public LogAppService(IMongoRepository<OperationLog> opsLogRepository
        , IMongoRepository<LoginLog> loginLogRepository
        , IMongoRepository<LoggerLog> nlogLogRepository)
    {
        _opsLogRepository = opsLogRepository;
        _loginLogRepository = loginLogRepository;
        _nlogLogRepository = nlogLogRepository;
    }

    public async Task<PageModelDto<LoginLogDto>> GetLoginLogsPagedAsync(LogSearchPagedDto searchDto)
    {
        searchDto.TrimStringFields();

        var builder = Builders<LoginLog>.Filter;
        var filterList = new List<FilterDefinition<LoginLog>>();
        filterList.AddIf(x => searchDto.BeginTime.HasValue, builder.Gte(l => l.CreateTime, searchDto.BeginTime));
        filterList.AddIf(x => searchDto.EndTime.HasValue, builder.Lte(l => l.CreateTime, searchDto.EndTime));
        filterList.AddIf(x => searchDto.Account.IsNotNullOrWhiteSpace(), builder.Eq(l => l.Account, searchDto.Account));
        filterList.AddIf(x => searchDto.Method.IsNotNullOrWhiteSpace(), builder.Eq(l => l.Device, searchDto.Device));
        var filterDefinition = filterList.IsNotNullOrEmpty() ? builder.And(filterList) : builder.Where(x => true);

        var pagedModel = await _loginLogRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, filterDefinition, x => x.CreateTime, false);
        var result = Mapper.Map<PageModelDto<LoginLogDto>>(pagedModel);

        return result;
    }

    public async Task<PageModelDto<OpsLogDto>> GetOpsLogsPagedAsync(LogSearchPagedDto searchDto)
    {
        searchDto.TrimStringFields();

        var builder = Builders<OperationLog>.Filter;
        var filterList = new List<FilterDefinition<OperationLog>>();
        filterList.AddIf(x => searchDto.BeginTime.HasValue, builder.Gte(l => l.CreateTime, searchDto.BeginTime));
        filterList.AddIf(x => searchDto.EndTime.HasValue, builder.Lte(l => l.CreateTime, searchDto.EndTime));
        filterList.AddIf(x => searchDto.Account.IsNotNullOrWhiteSpace(), builder.Eq(l => l.Account, searchDto.Account));
        filterList.AddIf(x => searchDto.Method.IsNotNullOrWhiteSpace(), builder.Eq(l => l.Method, searchDto.Method));
        var filterDefinition = filterList.IsNotNullOrEmpty() ? builder.And(filterList) : builder.Where(x => true);

        var pagedModel = await _opsLogRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, filterDefinition, x => x.CreateTime, false);
        var result = Mapper.Map<PageModelDto<OpsLogDto>>(pagedModel);

        return result;
    }

    public async Task<PageModelDto<NlogLogDto>> GetNlogLogsPagedAsync(LogSearchPagedDto searchDto)
    {
        searchDto.TrimStringFields();

        var builder = Builders<LoggerLog>.Filter;
        var filterList = new List<FilterDefinition<LoggerLog>>();
        filterList.AddIf(x => searchDto.BeginTime.HasValue, builder.Gte(l => l.Date, searchDto.BeginTime));
        filterList.AddIf(x => searchDto.EndTime.HasValue, builder.Lte(l => l.Date, searchDto.EndTime));
        filterList.AddIf(x => searchDto.Method.IsNotNullOrWhiteSpace(), builder.Eq(l => l.Properties.Method, searchDto.Method));
        filterList.AddIf(x => searchDto.Level.IsNotNullOrWhiteSpace(), builder.Eq(l => l.Level, searchDto.Level));
        var filterDefinition = filterList.Count > 0 ? builder.And(filterList) : builder.Where(x => true);

        var pagedModel = await _nlogLogRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, filterDefinition, x => x.Date, false);
        var result = Mapper.Map<PageModelDto<NlogLogDto>>(pagedModel);

        return result;
    }
}