namespace Adnc.Demo.Maint.Application.Services;

/// <summary>
/// 日志查询
/// </summary>
public interface ILogService : IAppService
{
    /// <summary>
    /// 登录日志
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageModelDto<LoginLogDto>> GetLoginLogsPagedAsync(SearchPagedDto input);

    /// <summary>
    /// 操作日志
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageModelDto<OperationLogDto>> GetOperationLogsPagedAsync(SearchPagedDto input);

    /*
    /// <summary>
    /// 异常日志
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageModelDto<NlogLogDto>> GetNlogLogsPagedAsync(SearchPagedDto input);
    */
}
