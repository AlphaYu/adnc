using Adnc.Shared.Application.Contracts;

namespace Adnc.Demo.Maint.WebApi.Controllers;

/// <summary>
/// 日志管理
/// </summary>
[Route($"{RouteConsts.MaintRoot}")]
public class LogController : AdncControllerBase
{
    private readonly ILogAppService _logService;
    private readonly UserContext _userContext;

    public LogController(ILogAppService logService
        , UserContext userContext)
    {
        _logService = logService;
        _userContext = userContext;
    }

    /// <summary>
    /// 查询操作日志
    /// </summary>
    /// <param name="searchDto">查询条件</param>
    /// <returns></returns>
    [HttpGet("opslogs")]
    [AdncAuthorize(PermissionConsts.Log.GetListForOperationLog)]
    public async Task<ActionResult<PageModelDto<OpsLogDto>>> GetOpsLogsPaged([FromQuery] LogSearchPagedDto searchDto)
        => await _logService.GetOpsLogsPagedAsync(searchDto);

    /// <summary>
    /// 查询用户操作日志
    /// </summary>
    /// <param name="searchDto">查询条件</param>
    /// <returns></returns>
    [HttpGet("users/opslogs")]
    public async Task<ActionResult<PageModelDto<OpsLogDto>>> GetUserOpsLogsPagedAsync([FromQuery] LogSearchPagedDto searchDto)
    {
        var logSearchDto = new LogSearchPagedDto()
        {
            Account = _userContext.Account,
            PageIndex = searchDto.PageIndex,
            PageSize = searchDto.PageSize
        };
        return await _logService.GetOpsLogsPagedAsync(logSearchDto);
    }

    /// <summary>
    /// 查询登录日志
    /// </summary>
    /// <param name="searchDto">查询条件</param>
    /// <returns></returns>
    [HttpGet("loginlogs")]
    [AdncAuthorize(PermissionConsts.Log.GetListForLogingLog)]
    public async Task<ActionResult<PageModelDto<LoginLogDto>>> GetLoginLogsPagedAsync([FromQuery] LogSearchPagedDto searchDto)
        => await _logService.GetLoginLogsPagedAsync(searchDto);

    /// <summary>
    /// 查询Nlog日志
    /// </summary>
    /// <param name="searchDto">查询条件</param>
    /// <returns></returns>
    [HttpGet("nloglogs")]
    [AdncAuthorize(PermissionConsts.Log.GetListForNLog)]
    public async Task<ActionResult<PageModelDto<NlogLogDto>>> GetNlogLogsPagedAsync([FromQuery] LogSearchPagedDto searchDto)
        => await _logService.GetNlogLogsPagedAsync(searchDto);
}