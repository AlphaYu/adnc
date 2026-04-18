using Adnc.Demo.Maint.Application.Contracts.Dtos.Log;

namespace Adnc.Demo.Maint.Api.Controllers;

/// <summary>
/// Manages login log queries.
/// </summary>
[Route($"{RouteConsts.MaintRoot}/loginlogs")]
public class LoginLogController(ILogService logService) : AdncControllerBase
{
    /// <summary>
    /// Gets a paged list of login logs.
    /// </summary>
    /// <param name="searchDto">The paging and filtering criteria.</param>
    /// <returns>A paged list of login logs.</returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Log.SearchForLogingLog)]
    public async Task<ActionResult<PageModelDto<LoginLogDto>>> GetLoginLogsPagedAsync([FromQuery] SearchPagedDto searchDto)
        => await logService.GetLoginLogsPagedAsync(searchDto);

    /*
    /// <summary>
    /// Gets a paged list of NLog entries.
    /// </summary>
    /// <param name="searchDto">The paging and filtering criteria.</param>
    /// <returns>A paged list of NLog entries.</returns>
    //[HttpGet("nloglogs")]
    //[AdncAuthorize(PermissionConsts.Log.GetListForNLog)]
    //public async Task<ActionResult<PageModelDto<NlogLogDto>>> GetNlogLogsPagedAsync([FromQuery] LogSearchPagedDto searchDto)
    //    => await logService.GetNlogLogsPagedAsync(searchDto);
    */
}
