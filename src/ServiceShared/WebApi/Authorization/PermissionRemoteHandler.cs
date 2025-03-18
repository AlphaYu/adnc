namespace Adnc.Shared.WebApi.Authorization;

public sealed class PermissionRemoteHandler(IAuthRestClient authRestClient) : AbstractPermissionHandler
{
    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        if (requestPermissions == null || !requestPermissions.Any())
            return await Task.FromResult(true);

        var permStr = string.Join("", requestPermissions);
        if (permStr.IsNullOrWhiteSpace())
            return await Task.FromResult(true);

        if (userBelongsRoleIds.IsNullOrWhiteSpace())
            return await Task.FromResult(false);

        //var jwtToken = await contextAccessor.HttpContext.GetTokenAsync("access_token");
        //var refitResult = await _authRpcService.GetCurrenUserPermissions($"Bearer {jwtToken}", userId, codes);
        var restResult = await authRestClient.GetCurrenUserPermissionsAsync(userId, requestPermissions, userBelongsRoleIds);
        if (!restResult.IsSuccessStatusCode)
            return false;

        return restResult.Content.IsNotNullOrEmpty();
    }
}