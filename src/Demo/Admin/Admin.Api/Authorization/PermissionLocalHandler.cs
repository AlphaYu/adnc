using Adnc.Shared.WebApi.Authorization.Handlers;

namespace Adnc.Demo.Admin.Api.Authorization.Handlers;

[Obsolete($"use {nameof(PermissionCacheHandler)} instead 2025 -02-17")]
public sealed class PermissionLocalHandler(IUserService userAppService) : AbstractPermissionHandler
{
    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        if (requestPermissions == null || !requestPermissions.Any())
        {
            return await Task.FromResult(true);
        }

        var permStr = string.Join("", requestPermissions);
        if (permStr.IsNullOrWhiteSpace())
        {
            return await Task.FromResult(true);
        }

        if (userBelongsRoleIds.IsNullOrWhiteSpace())
        {
            return await Task.FromResult(false);
        }

        var permissions = await userAppService.GetPermissionsAsync(userId, requestPermissions, userBelongsRoleIds);
        return permissions.IsNotNullOrEmpty();
    }
}
