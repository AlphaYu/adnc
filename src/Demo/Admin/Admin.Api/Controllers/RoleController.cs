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
    //[AdncAuthorize(PermissionConsts.Role.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<long>> CreateAsync([FromBody] RoleCreationDto input)
        => CreatedResult(await roleService.CreateAsync(input));

    /// <summary>
    /// 修改角色
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input">角色</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    //[AdncAuthorize(PermissionConsts.Role.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] RoleUpdationDto input)
        => Result(await roleService.UpdateAsync(id, input));

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="ids">角色ID</param>
    /// <returns></returns>
    [HttpDelete("{ids}")]
    //[AdncAuthorize(PermissionConsts.Role.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(",").Select(x => long.Parse(x)).ToArray();
        return Result(await roleService.DeleteAsync(idArr));
    }

    /// <summary>
    /// 获取角色信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [HttpPut("{id}/permissons")]
    //[AdncAuthorize(PermissionConsts.Role.SetPermissons)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SetPermissonsAsync([FromRoute] long id, [FromBody] long[] permissions)
        => Result(await roleService.SetPermissonsAsync(new RoleSetPermissonsDto() { RoleId = id, Permissions = permissions }));

    /// <summary>
    /// 获取角色权限
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns></returns>
    [HttpGet("{id}/menuids")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<string[]>> GetPermissions([FromRoute] long id)
        => await roleService.GetPermissionsAsync(id);

    /// <summary>
    /// 获取菜单选项
    /// </summary>
    /// <returns></returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OptionTreeDto>>> GetOptionsAsync()
        => await roleService.GetOptionsAsync();

    /// <summary>
    /// 查询角色
    /// </summary>
    /// <param name="input">角色查询条件</param>
    /// <returns></returns>
    [HttpGet("page")]
    //[AdncAuthorize(PermissionConsts.Role.GetList)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<RoleDto>>> GetPagedAsync([FromQuery] RolePagedSearchDto input)
        => await roleService.GetPagedAsync(input);
}