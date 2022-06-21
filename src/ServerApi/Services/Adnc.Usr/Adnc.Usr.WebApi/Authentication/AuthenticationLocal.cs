namespace Adnc.Usr.WebApi.Authentication;

public class AuthenticationLocal : IAuthentication
{
    private readonly IAccountAppService _accountAppService;

    public AuthenticationLocal(IAccountAppService accountAppService)
    {
        _accountAppService = accountAppService;
    }

    public async Task<Claim[]> ValidateAsync(string securityToken)
    {
        var claims = BearerTokenHelper.UnPackFromToken(securityToken);
        if(claims.IsNullOrEmpty())
            return Array.Empty<Claim>();

        var idClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId);
        if (idClaim is null)
            return Array.Empty<Claim>();

        var validatedInfo = await _accountAppService.GetUserValidatedInfoAsync(idClaim.Value.ToLong().Value);
        if (validatedInfo is null)
            return Array.Empty<Claim>();

        var jtiClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
        if (jtiClaim is null)
            return Array.Empty<Claim>();

        if (validatedInfo.ValidationVersion != jtiClaim.Value)
            return Array.Empty<Claim>();

        return claims.ToArray();
    }
}
