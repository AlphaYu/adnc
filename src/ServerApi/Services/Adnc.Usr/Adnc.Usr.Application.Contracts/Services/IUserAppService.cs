using System.Threading.Tasks;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Caching.Interceptor;
using Adnc.Usr.Application.Contracts.Consts;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Usr.Application.Contracts.Services
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
        [CachingEvict(CacheKeyPrefix = EasyCachingConsts.UserLoginInfoKeyPrefix)]
        Task<AppSrvResult> UpdateAsync([CachingParam]long id, UserUpdationDto input);

        [OpsLog(LogName = "删除用户")]
        [CachingEvict(CacheKeyPrefix = EasyCachingConsts.UserLoginInfoKeyPrefix)]
        Task<AppSrvResult> DeleteAsync([CachingParam] long id);

        [OpsLog(LogName = "设置用户角色")]
        //[CachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        //[CachingEvict(CacheKeyPrefix = EasyCachingConsts.UserLoginInfoKeyPrefix)]
        Task<AppSrvResult> SetRoleAsync([CachingParam] long id,UserSetRoleDto input);

        [OpsLog(LogName = "修改用户状态")]
        [CachingEvict(CacheKeyPrefix = EasyCachingConsts.UserLoginInfoKeyPrefix)]
        Task<AppSrvResult> ChangeStatusAsync([CachingParam] long id, int status);

        [OpsLog(LogName = "批量修改用户状态")]
        Task<AppSrvResult> ChangeStatusAsync(UserChangeStatusDto input);
    }
}