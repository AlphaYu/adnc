namespace Adnc.Demo.Admin.Api.Authorization;

[Obsolete($"use {nameof(PermissionCacheHandler)} instead 2025 -02-17")]
public sealed class PermissionLocalHandler : AbstractPermissionHandler
{
    private readonly IUserService _userAppService;

    public PermissionLocalHandler(IUserService userAppService) => _userAppService = userAppService;

    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        var permissions = await _userAppService.GetPermissionsAsync(userId, requestPermissions, userBelongsRoleIds);
        return permissions.IsNotNullOrEmpty();
    }
}