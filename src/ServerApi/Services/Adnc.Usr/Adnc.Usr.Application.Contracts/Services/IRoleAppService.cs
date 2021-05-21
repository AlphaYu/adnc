using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Caching.Interceptor;
using Adnc.Usr.Application.Contracts.Consts;
using Adnc.Usr.Application.Contracts.Dtos;
using System.Threading.Tasks;

namespace Adnc.Usr.Application.Contracts.Services
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
        [OpsLog(LogName = "新增角色")]
        [CachingEvict(CacheKey = CachingConsts.RoleListCacheKey)]
        Task<AppSrvResult<long>> CreateAsync(RoleCreationDto input);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "修改角色")]
        [CachingEvict(CacheKey = CachingConsts.RoleListCacheKey)]
        Task<AppSrvResult> UpdateAsync(long id, RoleUpdationDto input);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OpsLog(LogName = "删除角色")]
        [CachingEvict(CacheKeys = new[] { CachingConsts.MenuRelationCacheKey, CachingConsts.MenuCodesCacheKey, CachingConsts.RoleListCacheKey })]
        Task<AppSrvResult> DeleteAsync(long Id);

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "设置角色权限")]
        [CachingEvict(CacheKeys = new[] { CachingConsts.MenuRelationCacheKey, CachingConsts.MenuCodesCacheKey })]
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