namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// 角色管理
/// </summary>
[Route($"{RouteConsts.AdminRoot}/roles")]
[ApiController]
public class RoleController(IRoleService roleService) : AdncControllerBase
{
    /// <summary>
    /// 新增角色
    /// </summary>
    /// <param name="input">角色</param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Role.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] RoleCreationDto input)
        => CreatedResult(await roleService.CreateAsync(input));

    /// <summary>
    /// 修改角色
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input">角色</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Role.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] RoleUpdationDto input)
        => Result(await roleService.UpdateAsync(id, input));

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="ids">角色Id</param>
    /// <returns></returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.Role.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(",").Select(long.Parse).ToArray();
        return Result(await roleService.DeleteAsync(idArr));
    }

    /// <summary>
    /// 查询角色
    /// </summary>
    /// <param name="input">角色查询条件</param>
    /// <returns><see cref="PageModelDto{RoleDto}"/></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Role.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<RoleDto>>> GetPagedAsync([FromQuery] SearchPagedDto input)
        => await roleService.GetPagedAsync(input);

    /// <summary>
    /// 获取角色信息
    /// </summary>
    /// <param name="id">角色Id</param>
    /// <returns><see cref="RoleDto"/></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AdncAuthorize([PermissionConsts.Role.Get, PermissionConsts.Role.Update])]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> GetAsync([FromRoute] long id)
    {
        var role = await roleService.GetAsync(id);
        return role is null ? NotFound() : role;
    }

    /// <summary>
    /// 保存角色权限
    /// </summary>
    /// <param name="id">角色Id</param>
    /// <param name="permissions">用户权限Ids</param>
    /// <returns></returns>
    [HttpPatch("{id}/permissons")]
    [AdncAuthorize(PermissionConsts.Role.SetPermissons)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SetPermissonsAsync([FromRoute] long id, [FromBody] long[] permissions)
        => Result(await roleService.SetPermissonsAsync(new RoleSetPermissonsDto() { RoleId = id, Permissions = permissions }));

    /// <summary>
    /// 获取角色拥有的菜单Id
    /// </summary>
    /// <param name="id">角色Id</param>
    /// <returns></returns>
    [HttpGet("{id}/menuids")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<long[]>> GetMenuIdsAsync([FromRoute] long id)
        => await roleService.GetMenuIdsAsync(id);

    /// <summary>
    /// 获取菜单选项
    /// </summary>
    /// <returns><see cref="OptionTreeDto"/></returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OptionTreeDto>>> GetOptionsAsync()
        => await roleService.GetOptionsAsync();
}
