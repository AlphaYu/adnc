namespace Adnc.Shared.WebApi.Authentication.Bearer;

public static class JwtTokenHelper
{
    public static string GenerateJti() => Guid.NewGuid().ToString("N");

    /// <summary>
    /// create access token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="jti"></param>
    /// <param name="uniqueName"></param>
    /// <param name="nameId"></param>
    /// <param name="name"></param>
    /// <param name="roleIds"></param>
    /// <param name="loginerType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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
        {
            throw new ArgumentNullException(nameof(jti));
        }

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Jti,jti),
            new(JwtRegisteredClaimNames.UniqueName, uniqueName),
            new(JwtRegisteredClaimNames.NameId, nameId),
            new(JwtRegisteredClaimNames.Name, name),
            new(BearerDefaults.RoleIds, roleIds),
            new(BearerDefaults.LoginerType,loginerType)
        };
        return WriteToken(jwtConfig, claims, Tokens.AccessToken);
    }

    /// <summary>
    ///  create refresh token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="jti"></param>
    /// <param name="nameId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static JwtToken CreateRefreshToken(
        JWTOptions jwtConfig
        , string jti
        , string nameId
        )
    {
        if (jti.IsNullOrWhiteSpace())
        {
            throw new ArgumentNullException(nameof(jti));
        }

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Jti,jti),
            new(JwtRegisteredClaimNames.NameId, nameId),
        };
        return WriteToken(jwtConfig, claims, Tokens.RefreshToken);
    }

    /// <summary>
    /// get claim from refesh token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="refreshToken"></param>
    /// <param name="claimName"></param>
    /// <returns></returns>
    public static Claim? GetClaimFromRefeshToken(JWTOptions jwtConfig, string refreshToken, string claimName)
    {
        var parameters = jwtConfig.GenarateTokenValidationParameters();
        var tokenHandler = new JwtSecurityTokenHandler();
        var result = tokenHandler.ValidateToken(refreshToken, parameters, out var securityToken);
        if (result.Identity is null || !result.Identity.IsAuthenticated)
        {
            return null;
        }

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

        var issuer = jwtConfig.ValidIssuer;
        var audience = tokenType.Equals(Tokens.AccessToken) ? jwtConfig.ValidAudience : jwtConfig.RefreshTokenAudience;
        var seconds = tokenType.Equals(Tokens.AccessToken) ? jwtConfig.Expire : jwtConfig.RefreshTokenExpire;
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
