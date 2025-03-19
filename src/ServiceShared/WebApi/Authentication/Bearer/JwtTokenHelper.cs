namespace Adnc.Shared.WebApi.Authentication.Bearer;

public static class JwtTokenHelper
{
    public static string GenerateJti() => Guid.NewGuid().ToString("N");

    /// <summary>
    /// create access token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public static JwtToken CreateAccessToken(
        JWTOptions jwtConfig
        , string jti
        , string uniqueName
        , string nameId
        , string name
        , string roleIds
        , string loginerType)
    {
        if (jti.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(jti));

        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Jti,jti),
            new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName),
            new Claim(JwtRegisteredClaimNames.NameId, nameId),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim(BearerDefaults.RoleIds, roleIds),
            new Claim(BearerDefaults.LoginerType,loginerType)
        };
        return WriteToken(jwtConfig, claims, Tokens.AccessToken);
    }

    /// <summary>
    /// create refres token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public static JwtToken CreateRefreshToken(
        JWTOptions jwtConfig
        , string jti
        , string nameId
        )
    {
        if (jti.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(jti));

        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Jti,jti),
            new Claim(JwtRegisteredClaimNames.NameId, nameId),
        };
        return WriteToken(jwtConfig, claims, Tokens.RefreshToken);
    }

    /// <summary>
    /// get claim from refesh token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public static Claim? GetClaimFromRefeshToken(JWTOptions jwtConfig, string refreshToken, string claimName)
    {
        var parameters = jwtConfig.GenarateTokenValidationParameters();
        var tokenHandler = new JwtSecurityTokenHandler();
        var result = tokenHandler.ValidateToken(refreshToken, parameters, out var securityToken);
        if (result.Identity is null || !result.Identity.IsAuthenticated)
            return null;
        return result.Claims.FirstOrDefault(x => x.Type == claimName);
    }

    /// <summary>
    ///  write token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="claims"></param>
    /// <param name="tokenType"></param>
    /// <returns></returns>
    private static JwtToken WriteToken(JWTOptions jwtConfig, Claim[] claims, Tokens tokenType)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SymmetricSecurityKey));

        string issuer = jwtConfig.ValidIssuer;
        string audience = tokenType.Equals(Tokens.AccessToken) ? jwtConfig.ValidAudience : jwtConfig.RefreshTokenAudience;
        int seconds = tokenType.Equals(Tokens.AccessToken) ? jwtConfig.Expire : jwtConfig.RefreshTokenExpire;
        var expires = DateTime.Now.AddSeconds(seconds);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtToken(new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}
