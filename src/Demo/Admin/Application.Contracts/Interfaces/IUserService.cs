using Adnc.Demo.Admin.Application.Contracts.Dtos.User;

namespace Adnc.Demo.Admin.Application.Contracts.Interfaces;

/// <summary>
/// Defines user services.
/// </summary>
public interface IUserService : IAppService
{
    /// <summary>
    /// Creates a user.
    /// </summary>
    /// <param name="input">The user to create.</param>
    /// <returns>The ID of the created user.</returns>
    [OperateLog(LogName = "Create user")]
    [UnitOfWork]
    Task<ServiceResult<IdDto>> CreateAsync(UserCreationDto input);

    /// <summary>
    /// Updates a user.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="input">The user changes.</param>
    /// <returns>A result indicating whether the user was updated.</returns>
    [OperateLog(LogName = "Update user")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    [UnitOfWork]
    Task<ServiceResult> UpdateAsync([CachingParam] long id, UserUpdationDto input);

    /// <summary>
    /// Deletes one or more users.
    /// </summary>
    /// <param name="ids">The user IDs to delete.</param>
    /// <returns>A result indicating whether the users were deleted.</returns>
    [OperateLog(LogName = "Delete user")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    [UnitOfWork]
    Task<ServiceResult> DeleteAsync([CachingParam] long[] ids);

    /// <summary>
    /// Gets the permissions granted to a user from a requested permission set.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="requestPermissions">The permissions to check.</param>
    /// <param name="userBelongsRoleIds">The comma-separated role IDs assigned to the user.</param>
    /// <returns>The matching granted permissions.</returns>
    //[OperateLog(LogName = "Get current user permissions")]
    Task<List<string>> GetPermissionsAsync(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds);

    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The requested user, or <c>null</c> if it does not exist.</returns>
    Task<UserDto?> GetAsync(long id);

    /// <summary>
    /// Gets a paged list of users.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of users.</returns>
    Task<PageModelDto<UserDto>> GetPagedAsync(UserSearchPagedDto input);

    /// <summary>
    /// Gets the profile of the current user.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The user profile, or <c>null</c> if it does not exist.</returns>
    Task<UserProfileDto?> GetProfileAsync(long id);

    /// <summary>
    /// Updates the current user's profile.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="input">The profile changes.</param>
    /// <returns>A result indicating whether the profile was updated.</returns>
    [OperateLog(LogName = "Update current user information")]
    Task<ServiceResult> ChangeProfileAsync(long id, UserProfileUpdationDto input);

    /// <summary>
    /// Signs in a user.
    /// </summary>
    /// <param name="input">The login request.</param>
    /// <returns>The validated user information for the login session.</returns>
    //[OperateLog(LogName = "Sign in")]
    Task<ServiceResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input);

    /// <summary>
    /// Updates a user's password.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="input">The password change request.</param>
    /// <returns>A result indicating whether the password was updated.</returns>
    [OperateLog(LogName = "Change password")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    Task<ServiceResult> UpdatePasswordAsync([CachingParam] long id, UserProfileChangePwdDto input);

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="password">The new password.</param>
    /// <returns>A result indicating whether the password was reset.</returns>
    [OperateLog(LogName = "Reset password")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    Task<ServiceResult> ResetPasswordAsync([CachingParam] long id, string password);

    /// <summary>
    /// Gets cached validation info for a user.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The cached validation info, or <c>null</c> if it is not available.</returns>
    //[OperateLog(LogName = "Get validation info")]
    [CachingAble(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    Task<UserValidatedInfoDto?> GetUserValidatedInfoAsync([CachingParam] long id) => Task.FromResult<UserValidatedInfoDto?>(null);

    /// <summary>
    /// Removes cached validation info for a user.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>A result indicating whether the validation info was removed.</returns>
    [OperateLog(LogName = "Remove validation info")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.UserValidatedInfoKeyPrefix)]
    Task<ServiceResult> DeleteUserValidateInfoAsync([CachingParam] long id) => Task.FromResult(new ServiceResult());

    /// <summary>
    /// Extends the expiration time of cached validation info.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>A result indicating whether the cache expiration was updated.</returns>
    [OperateLog(LogName = "Extend validation info expiration")]
    Task<ServiceResult> ChangeUserValidateInfoExpiresDtAsync(long id);

    /// <summary>
    /// Gets user and permission info for the current context.
    /// </summary>
    /// <param name="userContext">The current user context.</param>
    /// <returns>The user and permission info, or <c>null</c> if it is unavailable.</returns>
    Task<UserInfoDto?> GetUserInfoAsync(UserContext userContext);
}
