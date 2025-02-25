namespace Adnc.Shared.WebApi.Authentication;

public abstract class AbstractAuthenticationProcessor
{
    public async Task<Claim[]> ValidateAsync(ClaimsPrincipal claimsPrincipal)
    {
        //var token = new JwtSecurityTokenHandler().ReadJwtToken(securityToken);
        //if (token is null || token.Claims.IsNullOrEmpty())
        //    return Array.Empty<Claim>();

        var claims = claimsPrincipal.Claims;

        var idClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId);
        if (idClaim is null)
            return Array.Empty<Claim>();

        var parseResult = long.TryParse(idClaim.Value, out long userId);
        if (parseResult == false)
            throw  new  InvalidCastException(nameof(idClaim.Value));

        var (ValidationVersion, Status) = await GetValidatedInfoAsync(userId);

        if (string.IsNullOrWhiteSpace(ValidationVersion) || Status != 1)
            return Array.Empty<Claim>();

        var jtiClaim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
        if (jtiClaim is null)
            return Array.Empty<Claim>();

        if (ValidationVersion != jtiClaim.Value)
            return Array.Empty<Claim>();

        return claims.ToArray();
    }

    protected abstract Task<(string? ValidationVersion, int Status)> GetValidatedInfoAsync(long userId);
}
