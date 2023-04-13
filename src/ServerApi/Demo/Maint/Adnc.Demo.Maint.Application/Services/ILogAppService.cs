namespace Adnc.Demo.Maint.Application.Services;

/// <summary>
/// 日志查询
/// </summary>
public interface ILogAppService : IAppService
{
    /// <summary>
    /// 登录日志
    /// </summary>
    /// <param name="searchDto"></param>
    /// <returns></returns>
    Task<PageModelDto<LoginLogDto>> GetLoginLogsPagedAsync(LogSearchPagedDto searchDto);

    /// <summary>
    /// 操作日志
    /// </summary>
    /// <param name="searchDto"></param>
    /// <returns></returns>
    Task<PageModelDto<OpsLogDto>> GetOpsLogsPagedAsync(LogSearchPagedDto searchDto);

    /// <summary>
    /// 异常日志
    /// </summary>
    /// <param name="searchDto"></param>
    /// <returns></returns>
    Task<PageModelDto<NlogLogDto>> GetNlogLogsPagedAsync(LogSearchPagedDto searchDto);
}