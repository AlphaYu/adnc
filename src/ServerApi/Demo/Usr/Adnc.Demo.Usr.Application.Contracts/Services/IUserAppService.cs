namespace Adnc.Demo.Usr.Application.Contracts.Services
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IUserAppService : IAppService
    {
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "新增用户")]
        Task<AppSrvResult<long>> CreateAsync(UserCreationDto input);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改用户")]
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
        Task<AppSrvResult> UpdateAsync([CachingParam] long id, UserUpdationDto input);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "删除用户")]
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
        Task<AppSrvResult> DeleteAsync([CachingParam] long id);

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "设置用户角色")]
        [CachingEvict(CacheKeys = new[] { CachingConsts.MenuRelationCacheKey, CachingConsts.MenuCodesCacheKey }
                             , CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
        Task<AppSrvResult> SetRoleAsync([CachingParam] long id, UserSetRoleDto input);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改用户状态")]
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
        Task<AppSrvResult> ChangeStatusAsync([CachingParam] long id, int status);

        /// <summary>
        /// 批量修改用户状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperateLog(LogName = "批量修改用户状态")]
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
        Task<AppSrvResult> ChangeStatusAsync([CachingParam] IEnumerable<long> ids, int status);

        /// <summary>
        /// 获取用户是否拥有指定权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestPermissions"></param>
        /// <param name="userBelongsRoleIds"></param>
        /// <returns></returns>
        //[OperateLog(LogName = "获取当前用户是否拥有指定权限")]
        Task<List<string>> GetPermissionsAsync(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<PageModelDto<UserDto>> GetPagedAsync(UserSearchPagedDto search);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserInfoDto> GetUserInfoAsync(long id);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[OperateLog(LogName = "登录")]
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