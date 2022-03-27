﻿namespace Adnc.Usr.Application.Contracts.Services
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
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidateInfoKeyPrefix)]
        Task<AppSrvResult> UpdateAsync([CachingParam] long id, UserUpdationDto input);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "删除用户")]
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidateInfoKeyPrefix)]
        Task<AppSrvResult> DeleteAsync([CachingParam] long id);

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "设置用户角色")]
        [CachingEvict(CacheKeys = new[] { CachingConsts.MenuRelationCacheKey, CachingConsts.MenuCodesCacheKey }
                             , CacheKeyPrefix = CachingConsts.UserValidateInfoKeyPrefix)]
        Task<AppSrvResult> SetRoleAsync([CachingParam] long id, UserSetRoleDto input);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改用户状态")]
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidateInfoKeyPrefix)]
        Task<AppSrvResult> ChangeStatusAsync([CachingParam] long id, int status);

        /// <summary>
        /// 批量修改用户状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [OperateLog(LogName = "批量修改用户状态")]
        [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidateInfoKeyPrefix)]
        Task<AppSrvResult> ChangeStatusAsync([CachingParam] IEnumerable<long> ids, int status);

        /// <summary>
        /// 获取当前用户是否拥有指定权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissions"></param>
        /// <param name="validationVersion"></param>
        /// <returns></returns>
        Task<List<string>> GetPermissionsAsync(long userId, IEnumerable<string> permissions, string validationVersion = null);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<PageModelDto<UserDto>> GetPagedAsync(UserSearchPagedDto search);
    }
}