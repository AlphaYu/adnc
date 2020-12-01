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
        Task<AppSrvResult<PageModelDto<RoleDto>>> GetPaged(RoleSearchDto searchDto);

        [OpsLog(LogName = "新增角色")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.RoleAllCacheKey)]
        Task<AppSrvResult<long>> Add(RoleSaveInputDto saveDto);

        [OpsLog(LogName = "修改角色")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.RoleAllCacheKey)]
        Task<AppSrvResult> Update(RoleSaveInputDto saveDto);

        [OpsLog(LogName = "删除角色")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey, EasyCachingConsts.RoleAllCacheKey })]
        Task<AppSrvResult> Delete(long Id);

        Task<AppSrvResult<dynamic>> GetRoleTreeListByUserId(long UserId);

        [OpsLog(LogName = "设置角色权限")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult> SaveRolePermisson(PermissonSaveInputDto inputDto);

        ValueTask<AppSrvResult<bool>> ExistPermissions(RolePermissionsCheckInputDto inputDto);

        Task<AppSrvResult<List<string>>> GetPermissions(RolePermissionsCheckInputDto inputDto);

        Task<List<RoleDto>> GetAllFromCache();
    }
}
