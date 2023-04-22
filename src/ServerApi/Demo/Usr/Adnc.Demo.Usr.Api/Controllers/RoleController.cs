namespace Adnc.Demo.Usr.Api.Controllers;

/// <summary>
/// 角色管理
/// </summary>
[Route($"{RouteConsts.UsrRoot}/roles")]
[ApiController]
public class RoleController : AdncControllerBase
{
    private readonly IRoleAppService _roleService;

    public RoleController(IRoleAppService roleService)
       => _roleService = roleService;

    /// <summary>
    /// 查询角色
    /// </summary>
    /// <param name="input">角色查询条件</param>
    /// <returns></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Role.GetList)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<RoleDto>>> GetPagedAsync([FromQuery] RolePagedSearchDto input)
        => await _roleService.GetPagedAsync(input);

    /// <summary>
    /// 根据用户ID获取角色树
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    [HttpGet("{userId}/rolestree")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleTreeDto>> GetRoleTreeListByUserIdAsync([FromRoute] long userId)
        => await _roleService.GetRoleTreeListByUserIdAsync(userId);

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [AdncAuthorize(PermissionConsts.Role.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        => Result(await _roleService.DeleteAsync(id));

    /// <summary>
    /// 保存角色权限
    /// </summary>
    /// <param name="id">角色Id</param>
    /// <param name="permissions">用户权限Ids</param>
    /// <returns></returns>
    [HttpPut("{id}/permissons")]
    [AdncAuthorize(PermissionConsts.Role.SetPermissons)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SetPermissonsAsync([FromRoute] long id, [FromBody] long[] permissions)
        => Result(await _roleService.SetPermissonsAsync(new RoleSetPermissonsDto() { RoleId = id, Permissions = permissions }));

    /// <summary>
    /// 新增角色
    /// </summary>
    /// <param name="input">角色</param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Role.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<long>> CreateAsync([FromBody] RoleCreationDto input)
        => CreatedResult(await _roleService.CreateAsync(input));

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
        => Result(await _roleService.UpdateAsync(id, input));
}