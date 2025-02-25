namespace Adnc.Demo.Usr.Application.Contracts.Services
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public interface IRoleAppService : IAppService
    {
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "新增角色")]
        [CachingEvict(CacheKey = CachingConsts.RoleListCacheKey)]
        Task<AppSrvResult<long>> CreateAsync(RoleCreationDto input);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改角色")]
        [CachingEvict(CacheKey = CachingConsts.RoleListCacheKey)]
        Task<AppSrvResult> UpdateAsync(long id, RoleUpdationDto input);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "删除角色")]
        [CachingEvict(CacheKeys = new[] { CachingConsts.MenuRelationCacheKey, CachingConsts.MenuCodesCacheKey, CachingConsts.RoleListCacheKey })]
        Task<AppSrvResult> DeleteAsync(long id);

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "设置角色权限")]
        [CachingEvict(CacheKeys = new[] { CachingConsts.MenuRelationCacheKey, CachingConsts.MenuCodesCacheKey })]
        [UnitOfWork]
        Task<AppSrvResult> SetPermissonsAsync(RoleSetPermissonsDto input);

        /// <summary>
        /// 获取用户拥有的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<RoleTreeDto> GetRoleTreeListByUserIdAsync(long userId);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageModelDto<RoleDto>> GetPagedAsync(RolePagedSearchDto input);
    }
}