using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Dtos;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Usr.Application.Services
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IUserAppService : IAppService
    {
        Task<AppSrvResult<PageModelDto<UserDto>>> GetPagedAsync(UserSearchPagedDto search);

        [OpsLog(LogName = "新增用户")]
        Task<AppSrvResult<long>> CreateAsync(UserCreationDto input);

        [OpsLog(LogName = "修改用户")]
        Task<AppSrvResult> UpdateAsync(long id, UserUpdationDto input);

        [OpsLog(LogName = "删除用户")]
        Task<AppSrvResult> DeleteAsync(long id);

        [OpsLog(LogName = "设置用户角色")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult> SetRoleAsync(long id,UserSetRoleDto input);

        [OpsLog(LogName = "修改用户状态")]
        Task<AppSrvResult> ChangeStatusAsync(long id, int status);

        [OpsLog(LogName = "批量修改用户状态")]
        Task<AppSrvResult> ChangeStatusAsync(UserChangeStatusDto input);
    }
}