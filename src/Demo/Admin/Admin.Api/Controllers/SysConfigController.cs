namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// 配置管理
/// </summary>
[Route($"{RouteConsts.AdminRoot}/sysconfigs")]
[ApiController]
public class SysConfigController(ISysConfigService sysConfigService) : AdncControllerBase
{
    /// <summary>
    /// 新增配置
    /// </summary>
    /// <param name="input"><see cref="SysConfigCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.SysConfig.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] SysConfigCreationDto input)
        => CreatedResult(await sysConfigService.CreateAsync(input));

    /// <summary>
    /// 更新配置
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input"><see cref="SysConfigUpdationDto"/></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.SysConfig.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] SysConfigUpdationDto input)
        => Result(await sysConfigService.UpdateAsync(id, input));

    /// <summary>
    /// 删除配置节点
    /// </summary>
    /// <param name="ids">节点id</param>
    /// <returns></returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.SysConfig.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(',').Select(long.Parse).ToArray();
        return Result(await sysConfigService.DeleteAsync(idArr));
    }

    /// <summary>
    /// 获取单个配置节点
    /// </summary>
    /// <param name="id">节点id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [AdncAuthorize([PermissionConsts.SysConfig.Get, PermissionConsts.SysConfig.Update])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SysConfigDto>> GetAsync([FromRoute] long id)
    {
        var cfg = await sysConfigService.GetAsync(id);
        return cfg is null ? NotFound() : cfg;
    }

    /// <summary>
    /// 获取配置列表
    /// </summary>
    /// <param name="input"><see cref="SearchPagedDto"/></param>
    /// <returns><see cref="PageModelDto{SysConfigDto}"/></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.SysConfig.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<SysConfigDto>>> GetPagedAsync([FromQuery] SearchPagedDto input)
      => await sysConfigService.GetPagedAsync(input);

    /// <summary>
    /// 根据keys获取配置列表
    /// </summary>
    /// <param name="keys"></param>
    /// <returns><see cref="List{SysConfigSimpleDto}"/></returns>
    [HttpGet()]
    [AdncAuthorize(PermissionConsts.SysConfig.Search, AdncAuthorizeAttribute.JwtWithBasicSchemes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SysConfigSimpleDto>>> GetListAsync([FromQuery] string keys)
        => await sysConfigService.GetListAsync(keys);
}
