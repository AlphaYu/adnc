using Adnc.Demo.Admin.Application.Contracts.Dtos.Notice;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages notices.
/// </summary>
[Route($"{RouteConsts.AdminRoot}/notices")]
[ApiController]
public class NoticeController() : AdncControllerBase
{
    /*
    /// <summary>
    /// Creates a notice.
    /// </summary>
    /// <param name="input">The notice to create.</param>
    /// <returns>The ID of the created notice.</returns>
    //[HttpPost]
    //[AdncAuthorize(PermissionConsts.SysConfig.Create)]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //public async Task<ActionResult<long>> CreateAsync([FromBody] SysConfigCreationDto input)
    //    =>  CreatedResult(await sysConfigService.CreateAsync(input));

    /// <summary>
    /// Updates a notice.
    /// </summary>
    /// <param name="id">The notice ID.</param>
    /// <param name="input">The notice changes.</param>
    /// <returns>A result indicating whether the notice was updated.</returns>
    //[HttpPut("{id}")]
    //[AdncAuthorize(PermissionConsts.SysConfig.Update)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] SysConfigUpdationDto input)
    //    => Result(await sysConfigService.UpdateAsync(id, input));

    /// <summary>
    /// Deletes one or more notices.
    /// </summary>
    /// <param name="ids">The comma-separated notice IDs.</param>
    /// <returns>A result indicating whether the notices were deleted.</returns>
    //[HttpDelete("{ids}")]
    //[AdncAuthorize(PermissionConsts.SysConfig.Delete)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    //{
    //    var idArr = ids.Split(',').Select(x => long.Parse(x)).ToArray();
    //    return Result(await sysConfigService.DeleteAsync(idArr));
    //}

    /// <summary>
    /// Gets a notice by ID.
    /// </summary>
    /// <param name="id">The notice ID.</param>
    /// <returns>The requested notice.</returns>
    //[HttpGet("{id}")]
    //// [AdncAuthorize(PermissionConsts.SysConfig.Search, AdncAuthorizeAttribute.JwtWithBasicSchemes)]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<SysConfigDto>> GetAsync([FromRoute] long id)
    //{
    //    var cfg = await sysConfigService.GetAsync(id);
    //    return cfg is null ? NotFound() : cfg;
    //}
    */

    /// <summary>
    /// Gets a paged list of notices for the current user.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of notices.</returns>
    [HttpGet("mine")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<NoticeDto>>> GetMinePagedAsync([FromQuery] NoticeSearchPagedDto input)
    {
        await Task.CompletedTask;
        return new PageModelDto<NoticeDto>(input);
    }
}
