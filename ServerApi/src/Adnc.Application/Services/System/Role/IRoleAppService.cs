using System.Threading;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Application.Services
{
    public interface IRoleAppService : IAppService
    {
        Task<PageModelDto<RoleDto>> GetPaged(RoleSearchDto searchDto);

        Task<int> Save(RoleSaveInputDto saveDto);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.MenuKesPrefix, IsAll = true)]
        Task<int> Delete(long Id);

        Task<dynamic> GetRoleTreeListByUserId(long UserId);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.MenuKesPrefix, IsAll = true)]
        Task<int> SaveRolePermisson(PermissonSaveInputDto inputDto);

        Task<bool> ExistPermissions(RolePermissionsCheckInputDto inputDto);
    }
}
