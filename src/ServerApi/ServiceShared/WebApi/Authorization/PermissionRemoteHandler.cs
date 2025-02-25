namespace Adnc.Shared.WebApi.Authorization;

public sealed class PermissionRemoteHandler : AbstractPermissionHandler
{
    private readonly IAuthRestClient _authRestClient;

    public PermissionRemoteHandler(IAuthRestClient authRestClient) => _authRestClient = authRestClient;

    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        //var jwtToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
        //var refitResult = await _authRpcService.GetCurrenUserPermissions($"Bearer {jwtToken}", userId, codes);
        var restResult = await _authRestClient.GetCurrenUserPermissionsAsync(userId, requestPermissions, userBelongsRoleIds);
        if (!restResult.IsSuccessStatusCode)
            return false;

        return restResult.Content.IsNotNullOrEmpty();
    }
}