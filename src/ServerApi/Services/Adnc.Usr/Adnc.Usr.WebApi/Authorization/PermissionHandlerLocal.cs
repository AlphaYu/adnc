namespace Adnc.Usr.WebApi.Authorization;

public sealed class PermissionHandlerLocal : AbstractPermissionHandler
{
    private readonly IUserAppService _userAppService;

    public PermissionHandlerLocal(IUserAppService userAppService) => _userAppService = userAppService;

    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        var permissions = await _userAppService.GetPermissionsAsync(userId, requestPermissions, userBelongsRoleIds);
        return permissions.IsNotNullOrEmpty();
    }
}