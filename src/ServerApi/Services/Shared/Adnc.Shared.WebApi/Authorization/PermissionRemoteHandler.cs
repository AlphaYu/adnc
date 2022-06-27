namespace Adnc.Shared.WebApi.Authorization;

public sealed class PermissionRemoteHandler : AbstractPermissionHandler
{
    private readonly IUsrRestClient _usrRestClient;

    public PermissionRemoteHandler(IUsrRestClient usrRestClient) => _usrRestClient = usrRestClient;

    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        //var jwtToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
        //var refitResult = await _authRpcService.GetCurrenUserPermissions($"Bearer {jwtToken}", userId, codes);
        var restResult = await _usrRestClient.GetCurrenUserPermissionsAsync(userId, requestPermissions, userBelongsRoleIds);
        if (!restResult.IsSuccessStatusCode)
            return false;

        return restResult.Content.IsNotNullOrEmpty();
    }
}