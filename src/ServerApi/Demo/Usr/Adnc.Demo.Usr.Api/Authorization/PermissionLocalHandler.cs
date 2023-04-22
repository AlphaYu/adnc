namespace Adnc.Demo.Usr.Api.Authorization;

public sealed class PermissionLocalHandler : AbstractPermissionHandler
{
    private readonly IUserAppService _userAppService;

    public PermissionLocalHandler(IUserAppService userAppService) => _userAppService = userAppService;

    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        var permissions = await _userAppService.GetPermissionsAsync(userId, requestPermissions, userBelongsRoleIds);
        return permissions.IsNotNullOrEmpty();
    }
}