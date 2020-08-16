using System.Threading;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Application.Interceptors.OpsLog;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Application.Services
{
    public interface IRoleAppService : IAppService
    {
        Task<PageModelDto<RoleDto>> GetPaged(RoleSearchDto searchDto);

        [OpsLog(LogName = "新增/修改角色")]
        Task Save(RoleSaveInputDto saveDto);

        [OpsLog(LogName = "删除角色")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task Delete(long Id);

        Task<dynamic> GetRoleTreeListByUserId(long UserId);

        [OpsLog(LogName = "设置角色权限")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task SaveRolePermisson(PermissonSaveInputDto inputDto);

        ValueTask<bool> ExistPermissions(RolePermissionsCheckInputDto inputDto);
    }
}
