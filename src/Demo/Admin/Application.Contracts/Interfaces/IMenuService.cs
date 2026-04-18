using Adnc.Demo.Admin.Application.Contracts.Dtos.Menu;

namespace Adnc.Demo.Admin.Application.Contracts.Interfaces;

/// <summary>
/// Defines menu and permission services.
/// </summary>
public interface IMenuService : IAppService
{
    /// <summary>
    /// Creates a menu.
    /// </summary>
    /// <param name="input">The menu to create.</param>
    /// <returns>The ID of the created menu.</returns>
    [OperateLog(LogName = "Create menu")]
    [CachingEvict(CacheKeys = new[] { CachingConsts.MenuListCacheKey })]
    Task<ServiceResult<IdDto>> CreateAsync(MenuCreationDto input);

    /// <summary>
    /// Updates a menu.
    /// </summary>
    /// <param name="id">The menu ID.</param>
    /// <param name="input">The menu changes.</param>
    /// <returns>A result indicating whether the menu was updated.</returns>
    [OperateLog(LogName = "Update menu")]
    [CachingEvict(CacheKeys = new[] { CachingConsts.MenuListCacheKey, CachingConsts.RoleMenuCodesCacheKey })]
    Task<ServiceResult> UpdateAsync(long id, MenuUpdationDto input);

    /// <summary>
    /// Deletes a menu.
    /// </summary>
    /// <param name="id">The menu ID.</param>
    /// <returns>A result indicating whether the menu was deleted.</returns>
    [OperateLog(LogName = "Delete menu")]
    [CachingEvict(CacheKeys = new[] { CachingConsts.MenuListCacheKey, CachingConsts.RoleMenuCodesCacheKey })]
    Task<ServiceResult> DeleteAsync(long id);

    /// <summary>
    /// Gets the menu tree.
    /// </summary>
    /// <param name="keywords">The optional keyword used to filter menus.</param>
    /// <returns>The menu tree.</returns>
    //[CachingAble(CacheKey = CachingConsts.MenuTreeListCacheKey, Expiration = CachingConsts.OneYear)]
    Task<List<MenuTreeDto>> GetTreelistAsync(string? keywords = null);

    /// <summary>
    /// Gets a menu by ID.
    /// </summary>
    /// <param name="id">The menu ID.</param>
    /// <returns>The requested menu, or <c>null</c> if it does not exist.</returns>
    Task<MenuDto?> GetAsync(long id);

    /// <summary>
    /// Gets router menus for the specified roles.
    /// </summary>
    /// <param name="roleIds">The role IDs used to filter menus.</param>
    /// <returns>The router menu tree for the roles.</returns>
    Task<List<RouterTreeDto>> GetMenusForRouterAsync(IEnumerable<long> roleIds);

    /// <summary>
    /// Gets menu options.
    /// </summary>
    /// <param name="onlyParent">Whether only parent-capable menus should be returned.</param>
    /// <returns>The menu option tree.</returns>
    Task<List<OptionTreeDto>> GetMenuOptionsAsync(bool? onlyParent);
}
