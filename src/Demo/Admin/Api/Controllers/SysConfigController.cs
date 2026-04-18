using Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages system configurations.
/// </summary>
[Route($"{RouteConsts.AdminRoot}/sysconfigs")]
[ApiController]
public class SysConfigController(ISysConfigService sysConfigService) : AdncControllerBase
{
    /// <summary>
    /// Creates a system configuration.
    /// </summary>
    /// <param name="input">The system configuration to create.</param>
    /// <returns>The ID of the created configuration.</returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.SysConfig.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] SysConfigCreationDto input)
        => CreatedResult(await sysConfigService.CreateAsync(input));

    /// <summary>
    /// Updates a system configuration.
    /// </summary>
    /// <param name="id">The configuration ID.</param>
    /// <param name="input">The configuration changes.</param>
    /// <returns>A result indicating whether the configuration was updated.</returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.SysConfig.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] SysConfigUpdationDto input)
        => Result(await sysConfigService.UpdateAsync(id, input));

    /// <summary>
    /// Deletes one or more system configurations.
    /// </summary>
    /// <param name="ids">The comma-separated configuration IDs.</param>
    /// <returns>A result indicating whether the configurations were deleted.</returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.SysConfig.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(',').Select(long.Parse).ToArray();
        return Result(await sysConfigService.DeleteAsync(idArr));
    }

    /// <summary>
    /// Gets a system configuration by ID.
    /// </summary>
    /// <param name="id">The configuration ID.</param>
    /// <returns>The requested system configuration.</returns>
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
    /// Gets a paged list of system configurations.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of system configurations.</returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.SysConfig.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<SysConfigDto>>> GetPagedAsync([FromQuery] SearchPagedDto input)
      => await sysConfigService.GetPagedAsync(input);

    /// <summary>
    /// Gets system configurations by key list.
    /// </summary>
    /// <param name="keys">The configuration keys, or <c>all</c> to get all items.</param>
    /// <returns>The matching system configurations.</returns>
    [HttpGet()]
    [AdncAuthorize(PermissionConsts.SysConfig.Search, AdncAuthorizeAttribute.JwtWithBasicSchemes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SysConfigSimpleDto>>> GetListAsync([FromQuery] string keys)
        => await sysConfigService.GetListAsync(keys);
}
