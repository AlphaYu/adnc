namespace Adnc.Demo.Maint.WebApi.Controllers;

/// <summary>
/// 操作日志管理
/// </summary>
[Route($"{RouteConsts.MaintRoot}/operationlogs")]
public class OperationLogController(ILogService logService, UserContext userContext) : AdncControllerBase
{
    /// <summary>
    /// 查询操作日志
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Log.SearchForOperationLog)]
    public async Task<ActionResult<PageModelDto<OperationLogDto>>> GetOpsLogsPaged([FromQuery] SearchPagedDto input)
        => await logService.GetOperationLogsPagedAsync(input);

    /// <summary>
    /// 查询登录用户操作日志
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns></returns>
    [HttpGet("user/page")]
    public async Task<ActionResult<PageModelDto<OperationLogDto>>> GetUserOpsLogsPagedAsync([FromQuery] SearchPagedDto input)
    {
        input.Keywords = userContext.Account;
        return await logService.GetOperationLogsPagedAsync(input);
    }
}
