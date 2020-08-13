using System.Threading;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Application.Services
{
    public interface IRoleAppService : IAppService
    {
        Task<PageModelDto<RoleDto>> GetPaged(RoleSearchDto searchDto);

        Task Save(RoleSaveInputDto saveDto);

        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task Delete(long Id);

        Task<dynamic> GetRoleTreeListByUserId(long UserId);

        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task SaveRolePermisson(PermissonSaveInputDto inputDto);

        ValueTask<bool> ExistPermissions(RolePermissionsCheckInputDto inputDto);
    }
}
