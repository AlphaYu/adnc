using Adnc.Demo.Admin.Application.Contracts.Dtos.Menu;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// 菜单管理
/// </summary>
[Route($"{RouteConsts.AdminRoot}/menus")]
[ApiController]
public class MenuController(IMenuService menuService, UserContext userContext) : AdncControllerBase
{
    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="input">菜单</param>
    /// <returns></returns>
    [HttpPost]
    //[AdncAuthorize(PermissionConsts.Menu.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<long>> CreateAsync([FromBody] MenuCreationDto input)
        => CreatedResult(await menuService.CreateAsync(input));

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input">菜单</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    //[AdncAuthorize(PermissionConsts.Menu.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] MenuUpdationDto input)
        => Result(await menuService.UpdateAsync(id, input));

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    //[AdncAuthorize(PermissionConsts.Menu.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        => Result(await menuService.DeleteAsync(id));

    /// <summary>
    /// 获取菜单信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    // ss[AdncAuthorize(PermissionConsts.Menu.GetList)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MenuDto>> GetAsync([FromRoute] long id)
    {
        var menu = await menuService.GetAsync(id);
        return menu is null ? NotFound() : menu;
    }

    /// <summary>
    /// 获取菜单树
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    // ss[AdncAuthorize(PermissionConsts.Menu.GetList)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MenuTreeDto>>> GetlistAsync(string? keywords = null)
        => await menuService.GetTreelistAsync(keywords);

    /// <summary>
    /// 获取侧边栏路由菜单
    /// </summary>
    /// <returns></returns>
    [HttpGet("routers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RouterTreeDto>>> GetMenusForRouterAsync()
    {
        string[] roleIds = userContext.RoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? [];
        return await menuService.GetMenusForRouterAsync(roleIds.Select(x=>long.Parse(x)));
    }

    /// <summary>
    /// 获取菜单选项
    /// </summary>
    /// <returns></returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OptionTreeDto>>> GetMenuOptionsAsync(bool? onlyParent)
    {
        string[] roleIds = userContext.RoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? [];
        return await menuService.GetMenuOptionsAsync(onlyParent);
    }
}