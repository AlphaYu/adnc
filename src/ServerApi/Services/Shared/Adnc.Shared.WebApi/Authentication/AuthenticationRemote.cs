using Adnc.Shared.Rpc.Rest.Services;
using Adnc.Shared.WebApi.Authentication.Bearer;

namespace Adnc.Shared.WebApi.Authentication;

public class AuthenticationRemote : IAuthentication
{
    public IAuthRestClient _authRestClient;

    public AuthenticationRemote(
        IAuthRestClient authRestClient
        )
    {
        _authRestClient = authRestClient;
    }

    public async Task<Claim[]> ValidateAsync(string securityToken)
    {
        var claims = BearerTokenHelper.UnPackFromToken(securityToken);
        if (claims.IsNullOrEmpty())
            return Array.Empty<Claim>();

        var apiReuslt = await _authRestClient.GetValidatedInfoAsync();
        if (!apiReuslt.IsSuccessStatusCode)
            return Array.Empty<Claim>();

        var idClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId);
        if (idClaim is null)
            return Array.Empty<Claim>();

        var validatedInfo = apiReuslt.Content;
        if (validatedInfo is null)
            return Array.Empty<Claim>();

        var jtiClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
        if (jtiClaim is null)
            return Array.Empty<Claim>();

        if (validatedInfo.ValidationVersion != jtiClaim.Value)
            return Array.Empty<Claim>();

        return claims;
    }
}
