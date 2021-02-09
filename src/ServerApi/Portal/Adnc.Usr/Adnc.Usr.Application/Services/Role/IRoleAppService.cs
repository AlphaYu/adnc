using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Dtos;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Usr.Application.Services
{
    public interface IRoleAppService : IAppService
    {
        Task<AppSrvResult<PageModelDto<RoleDto>>> GetPagedAsync(RolePagedSearchDto input);

        [OpsLog(LogName = "新增角色")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.RoleAllCacheKey)]
        Task<AppSrvResult<long>> CreateAsync(RoleCreationDto input);

        [OpsLog(LogName = "修改角色")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.RoleAllCacheKey)]
        Task<AppSrvResult> UpdateAsync(long id, RoleUpdationDto input);

        [OpsLog(LogName = "删除角色")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey, EasyCachingConsts.RoleAllCacheKey })]
        Task<AppSrvResult> DeleteAsync(long Id);

        Task<AppSrvResult<dynamic>> GetRoleTreeListByUserIdAsync(long userId);

        [OpsLog(LogName = "设置角色权限")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult> SetPermissonsAsync(RoleSetPermissonsDto input);

        ValueTask<AppSrvResult<bool>> ExistPermissionsAsync(RolePermissionsCheckerDto input);

        Task<AppSrvResult<List<string>>> GetPermissionsAsync(RolePermissionsCheckerDto input);

        Task<List<RoleDto>> GetAllFromCacheAsync();
    }
}
