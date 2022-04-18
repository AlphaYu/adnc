namespace Adnc.Usr.WebApi.Authorize;

public  static class JwtTokenHelper
{
    //认证服务器安装：System.IdentityModel.Tokens.Jwt
    //资源服务器安装：Microsoft.AspNetCore.Authentication.JwtBearer
    /// <summary>
    /// create access token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string CreateAccessToken(JwtConfig jwtConfig, UserValidateDto user)
    {
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Account),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim("version", user.ValidationVersion)
        };
        return WriteToken(jwtConfig, claims, Tokens.AccessToken);
    }

    /// <summary>
    /// create refres token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string CreateRefreshToken(JwtConfig jwtConfig, UserValidateDto user)
    {
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Account),
        };
        return WriteToken(jwtConfig, claims, Tokens.RefreshToken);
    }

    /// <summary>
    /// get account from refesh token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public static string GetAccountFromRefeshToken(string refreshToken)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
        if (token is not null)
        {
            var claimAccount = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
            return claimAccount;
        }
        return string.Empty;
    }

    /// <summary>
    ///  write token
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <param name="claims"></param>
    /// <param name="tokenType"></param>
    /// <returns></returns>
    private static string WriteToken(JwtConfig jwtConfig, Claim[] claims, Tokens tokenType)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SymmetricSecurityKey));

        string issuer = jwtConfig.Issuer;
        string audience = tokenType.Equals(Tokens.AccessToken) ? jwtConfig.Audience : jwtConfig.RefreshTokenAudience;
        int expires = tokenType.Equals(Tokens.AccessToken) ? jwtConfig.Expire : jwtConfig.RefreshTokenExpire;

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(expires),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var jwtAccessTokenToken = new JwtSecurityTokenHandler().WriteToken(token);
        return jwtAccessTokenToken;
    }
}