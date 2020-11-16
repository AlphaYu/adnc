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
        Task<PageModelDto<RoleDto>> GetPaged(RoleSearchDto searchDto);

        [OpsLog(LogName = "新增/修改角色")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.RoleAllCacheKey)]
        Task Save(RoleSaveInputDto saveDto);

        [OpsLog(LogName = "删除角色")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey, EasyCachingConsts.RoleAllCacheKey })]
        Task Delete(long Id);

        Task<dynamic> GetRoleTreeListByUserId(long UserId);

        [OpsLog(LogName = "设置角色权限")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task SaveRolePermisson(PermissonSaveInputDto inputDto);

        ValueTask<bool> ExistPermissions(RolePermissionsCheckInputDto inputDto);

        Task<IEnumerable<string>> GetPermissions(RolePermissionsCheckInputDto inputDto);

        Task<List<RoleDto>> GetAllFromCache();
    }
}
