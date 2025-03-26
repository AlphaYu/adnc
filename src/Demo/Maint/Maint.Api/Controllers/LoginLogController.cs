namespace Adnc.Demo.Maint.WebApi.Controllers;

/// <summary>
/// 登录日志管理
/// </summary>
[Route($"{RouteConsts.MaintRoot}/loginlogs")]
public class LoginLogController(ILogService logService) : AdncControllerBase
{
    /// <summary>
    /// 查询登录日志
    /// </summary>
    /// <param name="searchDto">查询条件</param>
    /// <returns></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Log.SearchForLogingLog)]
    public async Task<ActionResult<PageModelDto<LoginLogDto>>> GetLoginLogsPagedAsync([FromQuery] SearchPagedDto searchDto)
        => await logService.GetLoginLogsPagedAsync(searchDto);

    /*
    /// <summary>
    /// 查询Nlog日志
    /// </summary>
    /// <param name="searchDto">查询条件</param>
    /// <returns></returns>
    //[HttpGet("nloglogs")]
    //[AdncAuthorize(PermissionConsts.Log.GetListForNLog)]
    //public async Task<ActionResult<PageModelDto<NlogLogDto>>> GetNlogLogsPagedAsync([FromQuery] LogSearchPagedDto searchDto)
    //    => await logService.GetNlogLogsPagedAsync(searchDto);
    */
}
