using Adnc.Demo.Admin.Application.Contracts.Dtos.Menu;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages menus and router trees.
/// </summary>
[Route($"{RouteConsts.AdminRoot}/menus")]
[ApiController]
public class MenuController(IMenuService menuService, UserContext userContext) : AdncControllerBase
{
    /// <summary>
    /// Creates a menu.
    /// </summary>
    /// <param name="input">The menu to create.</param>
    /// <returns>The ID of the created menu.</returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Menu.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] MenuCreationDto input)
        => CreatedResult(await menuService.CreateAsync(input));

    /// <summary>
    /// Updates a menu.
    /// </summary>
    /// <param name="id">The menu ID.</param>
    /// <param name="input">The menu changes.</param>
    /// <returns>A result indicating whether the menu was updated.</returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Menu.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] MenuUpdationDto input)
        => Result(await menuService.UpdateAsync(id, input));

    /// <summary>
    /// Deletes a menu.
    /// </summary>
    /// <param name="id">The menu ID.</param>
    /// <returns>A result indicating whether the menu was deleted.</returns>
    [HttpDelete("{id}")]
    [AdncAuthorize(PermissionConsts.Menu.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        => Result(await menuService.DeleteAsync(id));

    /// <summary>
    /// Gets a menu by ID.
    /// </summary>
    /// <param name="id">The menu ID.</param>
    /// <returns>The requested menu.</returns>
    [HttpGet("{id}")]
    [AdncAuthorize([PermissionConsts.Menu.Get, PermissionConsts.Menu.Update])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuDto>> GetAsync([FromRoute] long id)
    {
        var menu = await menuService.GetAsync(id);
        return menu is null ? NotFound() : menu;
    }

    /// <summary>
    /// Gets the menu tree.
    /// </summary>
    /// <param name="keywords">The optional keyword used to filter menus.</param>
    /// <returns>The menu tree.</returns>
    [HttpGet()]
    //[AdncAuthorize(PermissionConsts.Menu.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MenuTreeDto>>> GetTreelistAsync(string? keywords = null)
        => await menuService.GetTreelistAsync(keywords);

    /// <summary>
    /// Gets the sidebar router menu tree for the current user.
    /// </summary>
    /// <returns>The router tree available to the current user.</returns>
    [HttpGet("routers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RouterTreeDto>>> GetMenusForRouterAsync()
    {
        var roleIds = userContext.RoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? [];
        return await menuService.GetMenusForRouterAsync(roleIds.Select(long.Parse));
    }

    /// <summary>
    /// Gets menu options.
    /// </summary>
    /// <param name="onlyParent">Whether to return only parent-capable menu nodes.</param>
    /// <returns>The menu option tree.</returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OptionTreeDto>>> GetMenuOptionsAsync(bool? onlyParent)
    {
        return await menuService.GetMenuOptionsAsync(onlyParent);
    }
}
