namespace Adnc.Demo.Maint.Application.Services.Implements;

public class LogService(IAdoQuerierRepository adoRepository) : AbstractAppService, ILogService
{
    public async Task<PageModelDto<LoginLogDto>> GetLoginLogsPagedAsync(LogSearchPagedDto search)
    {
        search.TrimStringFields();
        var where = new StringBuilder(100)
            .AppendIf(search.Account.IsNotNullOrWhiteSpace(), " AND account = @account")
            .AppendIf(search.Device.IsNotNullOrWhiteSpace(), " AND device = @device")
            .ToSqlWhereString();
        var orderBy = " ORDER BY id Desc";

        var queryCondition = new QueryCondition(where, orderBy, null, search);
        var queryResult = await adoRepository.GetPagedLoginLogsBySqlAsync<LoginLogDto>(queryCondition, search.SkipRows(), search.PageSize);
        return new PageModelDto<LoginLogDto>(search, queryResult.Content.ToArray(), queryResult.TotalCount);
    }

    public async Task<PageModelDto<OpsLogDto>> GetOpsLogsPagedAsync(LogSearchPagedDto search)
    {
        search.TrimStringFields();
        var where = new StringBuilder(100)
            .AppendIf(search.Account.IsNotNullOrWhiteSpace(), " AND account = @account")
            .AppendIf(search.Device.IsNotNullOrWhiteSpace(), " AND device = @device")
            .AppendIf(search.Device.IsNotNullOrWhiteSpace(), " AND method = @method")
            .AppendIf(search.Level.IsNotNullOrWhiteSpace(), " AND level = @level")
            .ToSqlWhereString();
        var orderBy = " ORDER BY id Desc";

        var queryCondition = new QueryCondition(where, orderBy, null, search);
        var queryResult = await adoRepository.GetPagedOperationLogsBySqlAsync<OpsLogDto>(queryCondition, search.SkipRows(), search.PageSize);
        return new PageModelDto<OpsLogDto>(search, queryResult.Content.ToArray(), queryResult.TotalCount);
    }

    //public async Task<PageModelDto<NlogLogDto>> GetNlogLogsPagedAsync(LogSearchPagedDto searchDto)
    //{
    //    searchDto.TrimStringFields();

    //    var builder = Builders<LoggerLog>.Filter;
    //    var filterList = new List<FilterDefinition<LoggerLog>>();
    //    filterList.AddIf(x => searchDto.BeginTime.HasValue, builder.Gte(l => l.Date, searchDto.BeginTime));
    //    filterList.AddIf(x => searchDto.EndTime.HasValue, builder.Lte(l => l.Date, searchDto.EndTime));
    //    filterList.AddIf(x => searchDto.Method.IsNotNullOrWhiteSpace(), builder.Eq(l => l.Properties.Method, searchDto.Method));
    //    filterList.AddIf(x => searchDto.Level.IsNotNullOrWhiteSpace(), builder.Eq(l => l.Level, searchDto.Level));
    //    var filterDefinition = filterList.Count > 0 ? builder.And(filterList) : builder.Where(x => true);

    //    var pagedModel = await _nlogLogRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, filterDefinition, x => x.Date, false);
    //    var result = Mapper.Map<PageModelDto<NlogLogDto>>(pagedModel);

    //    return result;
    //}
}