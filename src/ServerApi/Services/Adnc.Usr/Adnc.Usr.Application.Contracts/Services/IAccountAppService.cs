namespace Adnc.Usr.Application.Contracts.Services
{
    /// <summary>
    /// 认证服务
    /// </summary>
    public interface IAccountAppService : IAppService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "登录")]
        Task<AppSrvResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改密码")]
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
        Task<AppSrvResult> UpdatePasswordAsync([CachingParam] long id, UserChangePwdDto input);

        /// <summary>
        /// 获取认证信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[OperateLog(LogName = "获取认证信息")]
        [CachingAble(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
        Task<UserValidatedInfoDto> GetUserValidatedInfoAsync([CachingParam] long id) => Task.FromResult<UserValidatedInfoDto>(null);

        /// <summary>
        /// 移除认证信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "移除认证信息")]
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
        Task<AppSrvResult> DeleteUserValidateInfoAsync([CachingParam] long id) => Task.FromResult(new AppSrvResult());

        /// <summary>
        /// 调整认证信息过期是时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "调整认证信息过期时间")]
        Task<AppSrvResult> ChangeUserValidateInfoExpiresDtAsync(long id);
    }
}