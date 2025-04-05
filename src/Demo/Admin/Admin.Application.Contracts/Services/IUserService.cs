namespace Adnc.Demo.Admin.Application.Contracts.Services;

/// <summary>
/// 用户管理
/// </summary>
public interface IUserService : IAppService
{
    /// <summary>
    /// 新增用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增用户")]
    [UnitOfWork]
    Task<ServiceResult<IdDto>> CreateAsync(UserCreationDto input);

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改用户")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    [UnitOfWork]
    Task<ServiceResult> UpdateAsync([CachingParam] long id, UserUpdationDto input);

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除用户")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    [UnitOfWork]
    Task<ServiceResult> DeleteAsync([CachingParam] long[] ids);

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
    /// 获取用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserDto?> GetAsync(long id);

    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageModelDto<UserDto>> GetPagedAsync(UserSearchPagedDto input);

    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserProfileDto?> GetProfileAsync(long id);

    /// <summary>
    /// 修改当前用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改当前用户信息")]
    Task<ServiceResult> ChangeProfileAsync(long id, UserProfileUpdationDto input);

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[OperateLog(LogName = "登录")]
    Task<ServiceResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input);

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改密码")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    Task<ServiceResult> UpdatePasswordAsync([CachingParam] long id, UserProfileChangePwdDto input);

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [OperateLog(LogName = "重置密码")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    Task<ServiceResult> ResetPasswordAsync([CachingParam] long id, string password);

    /// <summary>
    /// 获取认证信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    //[OperateLog(LogName = "获取认证信息")]
    [CachingAble(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    Task<UserValidatedInfoDto?> GetUserValidatedInfoAsync([CachingParam] long id) => Task.FromResult<UserValidatedInfoDto?>(null);

    /// <summary>
    /// 移除认证信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [OperateLog(LogName = "移除认证信息")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    Task<ServiceResult> DeleteUserValidateInfoAsync([CachingParam] long id) => Task.FromResult(new ServiceResult());

    /// <summary>
    /// 调整认证信息过期是时间
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [OperateLog(LogName = "调整认证信息过期时间")]
    Task<ServiceResult> ChangeUserValidateInfoExpiresDtAsync(long id);

    /// <summary>
    /// 获取用户与权限信息
    /// </summary>
    /// <returns></returns>
    Task<UserInfoDto?> GetUserInfoAsync(UserContext userContext);
}
