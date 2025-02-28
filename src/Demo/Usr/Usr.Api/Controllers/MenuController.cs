namespace Adnc.Demo.Usr.Api.Controllers;

/// <summary>
/// 菜单管理
/// </summary>
[Route($"{RouteConsts.UsrRoot}/menus")]
[ApiController]
public class MenuController : AdncControllerBase
{
    private readonly IMenuAppService _menuService;
    private readonly UserContext _userContext;

    public MenuController(
        IMenuAppService menuService
        , UserContext userContext)
    {
        _menuService = menuService;
        _userContext = userContext;
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="menuDto">菜单</param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Menu.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<long>> CreateAsync([FromBody] MenuCreationDto menuDto)
        => CreatedResult(await _menuService.CreateAsync(menuDto));

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input">菜单</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Menu.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] MenuUpdationDto input)
        => Result(await _menuService.UpdateAsync(id, input));

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [AdncAuthorize(PermissionConsts.Menu.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        => Result(await _menuService.DeleteAsync(id));

    /// <summary>
    /// 获取菜单树
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [AdncAuthorize(PermissionConsts.Menu.GetList)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MenuNodeDto>>> GetlistAsync()
        => await _menuService.GetlistAsync();

    /// <summary>
    /// 获取侧边栏路由菜单
    /// </summary>
    /// <returns></returns>
    [HttpGet("routers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MenuRouterDto>>> GetMenusForRouterAsync()
    {
        var roleIds = _userContext.RoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        return await _menuService.GetMenusForRouterAsync(roleIds.Select(x => long.Parse(x)));
    }

    /// <summary>
    /// 根据角色获取菜单树
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns></returns>
    [HttpGet("{roleId}/menutree")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MenuTreeDto>> GetMenuTreeListByRoleIdAsync([FromRoute] long roleId)
        => await _menuService.GetMenuTreeListByRoleIdAsync(roleId);
}