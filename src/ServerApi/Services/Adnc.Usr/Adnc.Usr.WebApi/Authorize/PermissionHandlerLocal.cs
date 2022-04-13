namespace Microsoft.AspNetCore.Authorization;

public class PermissionHandlerLocal : AbstractPermissionHandler
{
    private readonly IUserAppService _userAppService;

    public PermissionHandlerLocal(IUserAppService userAppService)
        => _userAppService = userAppService;

    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> codes, string validationVersion)
    {
        var permissions = await _userAppService.GetPermissionsAsync(userId, codes, validationVersion);
        return permissions.IsNotNullOrEmpty();
    }
}