using System.Threading.Tasks;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Usr.Application.Contracts.Consts;
using Adnc.Infra.Caching.Interceptor;

namespace Adnc.Usr.Application.Contracts.Services
{
    /// <summary>
    /// 账户服务
    /// </summary>
    public interface IAccountAppService : IAppService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AppSrvResult<UserValidateDto>> LoginAsync(UserLoginDto input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "修改密码")]
        [CachingEvict(CacheKeyPrefix =CachingConsts.UserLoginInfoKeyPrefix)]
        Task<AppSrvResult> UpdatePasswordAsync([CachingParam] long id, UserChangePwdDto input);

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CachingAble(CacheKeyPrefix = CachingConsts.UserLoginInfoKeyPrefix)]
        Task<UserValidateDto> GetUserValidateInfoAsync([CachingParam] long id);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserInfoDto> GetUserInfoAsync(long userId);
    }
}
