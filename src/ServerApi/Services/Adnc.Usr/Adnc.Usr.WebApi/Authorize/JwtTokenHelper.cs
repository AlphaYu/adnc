namespace Adnc.Usr.WebApi.Authorize;

//认证服务器安装：System.IdentityModel.Tokens.Jwt
//资源服务器安装：Microsoft.AspNetCore.Authentication.JwtBearer
public enum TokenType
{
    AccessToken = 1,
    RefreshToken = 2
}

public static class JwtTokenHelper
{
    public static string CreateToken(JwtConfig jwtConfig, Claim[] claims, TokenType tokenType)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SymmetricSecurityKey));

        string issuer = jwtConfig.Issuer;
        string audience = tokenType.Equals(TokenType.AccessToken) ? jwtConfig.Audience : jwtConfig.RefreshTokenAudience;
        int expires = tokenType.Equals(TokenType.AccessToken) ? jwtConfig.Expire : jwtConfig.RefreshTokenExpire;

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

    public static string CreateAccessToken(JwtConfig jwtConfig, UserValidateDto user)
    {
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Account),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim("version", user.ValidationVersion)
        };
        return CreateToken(jwtConfig, claims, TokenType.AccessToken);
    }

    public static string CreateRefreshToken(JwtConfig jwtConfig, UserValidateDto user)
    {
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Account),
        };
        return CreateToken(jwtConfig, claims, TokenType.RefreshToken);
    }

    public static string CreateAccessToken(JwtConfig jwtConfig, UserValidateDto user, string refreshTokenTxt)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenTxt);
        if (token != null)
        {
            var claimAccount = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;

            if (user != null && user.Account == claimAccount)
            {
                return CreateAccessToken(jwtConfig, user);
            }
        }
        return string.Empty;
    }
}