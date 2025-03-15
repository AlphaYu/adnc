namespace Adnc.Shared.WebApi.Authorization;

public sealed class PermissionRemoteHandler(IAuthRestClient authRestClient) : AbstractPermissionHandler
{
    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        //var jwtToken = await contextAccessor.HttpContext.GetTokenAsync("access_token");
        //var refitResult = await _authRpcService.GetCurrenUserPermissions($"Bearer {jwtToken}", userId, codes);
        var restResult = await authRestClient.GetCurrenUserPermissionsAsync(userId, requestPermissions, userBelongsRoleIds);
        if (!restResult.IsSuccessStatusCode)
            return false;

        return restResult.Content.IsNotNullOrEmpty();
    }
}