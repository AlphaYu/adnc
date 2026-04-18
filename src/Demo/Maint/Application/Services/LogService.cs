using System.Text;
using Adnc.Demo.Maint.Application.Contracts.Dtos.Log;

namespace Adnc.Demo.Maint.Application.Services;

/// <inheritdoc cref="ILogService"/>
public class LogService(IAdoQuerierRepository adoRepo) : AbstractAppService, ILogService
{
    /// <inheritdoc />
    public async Task<PageModelDto<LoginLogDto>> GetLoginLogsPagedAsync(SearchPagedDto input)
    {
        input.TrimStringFields();

        if (input.CreateTime is null || input.CreateTime.Length < 1)
        {
            var startTime = DateTime.Now.AddDays(-7);
            var endTime = DateTime.Now;
            input.CreateTime = [startTime, endTime];
        }

        var where = new StringBuilder(100)
            //.Append("AND CreateTime BETWEEN  '@CreateTime[0]' AND '@CreateTime[1]'")
            .Append($"AND CreateTime>='{input.CreateTime[0].ToStandardTimeString()}' AND CreateTime<='{input.CreateTime[1].ToStandardTimeString()}'")
            .AppendIf(input.Keywords.IsNotNullOrWhiteSpace(), " AND Account = @Keywords")
            .ToSqlWhereString();
        var orderBy = " ORDER BY id Desc";

        var queryCondition = new QueryCondition(where, orderBy, null, input);
        var queryResult = await adoRepo.GetPagedLoginLogsBySqlAsync<LoginLogDto>(queryCondition, input.SkipRows(), input.PageSize);
        return new PageModelDto<LoginLogDto>(input, queryResult.Content.ToArray(), queryResult.TotalCount);
    }

    /// <inheritdoc />
    public async Task<PageModelDto<OperationLogDto>> GetOperationLogsPagedAsync(SearchPagedDto input)
    {
        input.TrimStringFields();

        if (input.CreateTime is null || input.CreateTime.Length < 1)
        {
            var startTime = DateTime.Now.AddDays(-7);
            var endTime = DateTime.Now;
            input.CreateTime = [startTime, endTime];
        }

        var where = new StringBuilder(100)
            //.Append("AND CreateTime>=@CreateTime[0] AND CreateTime<=@CreateTime[1]")
            .Append($"AND CreateTime>='{input.CreateTime[0].ToStandardTimeString()}' AND CreateTime<='{input.CreateTime[1].ToStandardTimeString()}'")
            .AppendIf(input.Keywords.IsNotNullOrWhiteSpace(), " AND LogName = @Keywords")
            .ToSqlWhereString();
        var orderBy = " ORDER BY id Desc";

        var queryCondition = new QueryCondition(where, orderBy, null, input);
        var queryResult = await adoRepo.GetPagedOperationLogsBySqlAsync<OperationLogDto>(queryCondition, input.SkipRows(), input.PageSize);
        return new PageModelDto<OperationLogDto>(input, queryResult.Content.ToArray(), queryResult.TotalCount);
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
