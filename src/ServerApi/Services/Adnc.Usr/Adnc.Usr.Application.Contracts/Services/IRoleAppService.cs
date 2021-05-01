using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Infra.Caching.Interceptor;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Dtos;
using Adnc.Usr.Application.Contracts.Consts;

namespace Adnc.Usr.Application.Contracts.Services
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public interface IRoleAppService : IAppService
    {
        Task<AppSrvResult<PageModelDto<RoleDto>>> GetPagedAsync(RolePagedSearchDto input);

        [OpsLog(LogName = "新增角色")]
        [CachingEvict(CacheKey = EasyCachingConsts.RoleAllCacheKey)]
        Task<AppSrvResult<long>> CreateAsync(RoleCreationDto input);

        [OpsLog(LogName = "修改角色")]
        [CachingEvict(CacheKey = EasyCachingConsts.RoleAllCacheKey)]
        Task<AppSrvResult> UpdateAsync(long id, RoleUpdationDto input);

        [OpsLog(LogName = "删除角色")]
        [CachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey, EasyCachingConsts.RoleAllCacheKey })]
        Task<AppSrvResult> DeleteAsync(long Id);

        Task<AppSrvResult<RoleTreeDto>> GetRoleTreeListByUserIdAsync(long userId);

        [OpsLog(LogName = "设置角色权限")]
        [CachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult> SetPermissonsAsync(RoleSetPermissonsDto input);

        Task<bool> ExistPermissionsAsync(long userId, IEnumerable<string> permissions);

        Task<List<string>> GetPermissionsAsync(IEnumerable<long> roleIds, IEnumerable<string> permissions);
    }
}
