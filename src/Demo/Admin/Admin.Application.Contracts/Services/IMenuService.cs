namespace Adnc.Demo.Admin.Application.Contracts.Services;

/// <summary>
/// 菜单/权限服务
/// </summary>
public interface IMenuService : IAppService
{
    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增菜单")]
    [CachingEvict(CacheKeys = new[] { CachingConsts.MenuListCacheKey })]
    Task<ServiceResult<IdDto>> CreateAsync(MenuCreationDto input);

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改菜单")]
    [CachingEvict(CacheKeys = new[] { CachingConsts.MenuListCacheKey, CachingConsts.RoleMenuCodesCacheKey })]
    Task<ServiceResult> UpdateAsync(long id, MenuUpdationDto input);

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除菜单")]
    [CachingEvict(CacheKeys = new[] { CachingConsts.MenuListCacheKey, CachingConsts.RoleMenuCodesCacheKey })]
    Task<ServiceResult> DeleteAsync(long id);

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    //[CachingAble(CacheKey = CachingConsts.MenuTreeListCacheKey, Expiration = CachingConsts.OneYear)]
    Task<List<MenuTreeDto>> GetTreelistAsync(string? keywords = null);

    /// <summary>
    /// 获取菜单信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<MenuDto?> GetAsync(long id);

    /// <summary>
    /// 获取左侧路由菜单
    /// </summary>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    Task<List<RouterTreeDto>> GetMenusForRouterAsync(IEnumerable<long> roleIds);

    /// <summary>
    /// 获取菜单选项
    /// </summary>
    /// <param name="onlyParent"></param>
    /// <returns></returns>
    Task<List<OptionTreeDto>> GetMenuOptionsAsync(bool? onlyParent);
}
