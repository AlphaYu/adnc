namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// 通知管理
/// </summary>
[Route($"{RouteConsts.AdminRoot}/notices")]
[ApiController]
public class NoticeController() : AdncControllerBase
{
    /*
    /// <summary>
    /// 新增通知
    /// </summary>
    /// <param name="input"><see cref="SysConfigCreationDto"/></param>
    /// <returns></returns>
    //[HttpPost]
    //[AdncAuthorize(PermissionConsts.SysConfig.Create)]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //public async Task<ActionResult<long>> CreateAsync([FromBody] SysConfigCreationDto input)
    //    =>  CreatedResult(await sysConfigService.CreateAsync(input));

    /// <summary>
    /// 更新通知
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input"><see cref="SysConfigUpdationDto"/></param>
    /// <returns></returns>
    //[HttpPut("{id}")]
    //[AdncAuthorize(PermissionConsts.SysConfig.Update)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] SysConfigUpdationDto input)
    //    => Result(await sysConfigService.UpdateAsync(id, input));

    /// <summary>
    /// 删除通知
    /// </summary>
    /// <param name="ids">节点id</param>
    /// <returns></returns>
    //[HttpDelete("{ids}")]
    //[AdncAuthorize(PermissionConsts.SysConfig.Delete)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    //{
    //    var idArr = ids.Split(',').Select(x => long.Parse(x)).ToArray();
    //    return Result(await sysConfigService.DeleteAsync(idArr));
    //}

    /// <summary>
    /// 获取单个通知
    /// </summary>
    /// <param name="id">节点id</param>
    /// <returns></returns>
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
    /// 获取通知分页列表
    /// </summary>
    /// <param name="input"><see cref="NoticeSearchPagedDto"/></param>
    /// <returns><see cref="PageModelDto{CfgDto}"/></returns>
    [HttpGet("mine")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<NoticeDto>>> GetMinePagedAsync([FromQuery] NoticeSearchPagedDto input)
    {
        await Task.CompletedTask;
        return new PageModelDto<NoticeDto>(input);
    }
}
