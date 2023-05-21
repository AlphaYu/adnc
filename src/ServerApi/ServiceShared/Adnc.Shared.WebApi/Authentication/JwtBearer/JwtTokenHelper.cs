using NLog.LayoutRenderers;

namespace Adnc.Shared.WebApi.Authentication.JwtBearer;

public static class JwtTokenHelper
{
    /// <summary>
    ///  Create  a TokenValidationParameters instance
    /// </summary>
    /// <param name="tokenConfig"></param>
    /// <returns></returns>
    public static TokenValidationParameters GenarateTokenValidationParameters(JWTOptions tokenConfig) =>
        new()
        {
            ValidateIssuer = tokenConfig.ValidateIssuer,
            ValidIssuer = tokenConfig.ValidIssuer,
            ValidateIssuerSigningKey = tokenConfig.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(tokenConfig.Encoding.GetBytes(tokenConfig.SymmetricSecurityKey)),
            ValidateAudience = tokenConfig.ValidateAudience,
            ValidAudience = tokenConfig.ValidAudience,
            ValidateLifetime = tokenConfig.ValidateLifetime,
            RequireExpirationTime = tokenConfig.RequireExpirationTime,
            ClockSkew = TimeSpan.FromSeconds(tokenConfig.ClockSkew),
            //AudienceValidator = (m, n, z) =>m != null && m.FirstOrDefault().Equals(Const.ValidAudience)
        };

    /// <summary>
    /// Create a JwtBearerEvents instance
    /// </summary>
    /// <returns></returns>
    public static JwtBearerEvents GenarateJwtBearerEvents() =>
        new()
        {
            //接受到消息时调用
            OnMessageReceived = context => Task.CompletedTask
                ,
            //在Token验证通过后调用
            OnTokenValidated = context =>
            {
                var userContext = context.HttpContext.RequestServices.GetService<UserContext>() ?? throw new NullReferenceException(nameof(UserContext));
                var principal = context.Principal ?? throw new NullReferenceException(nameof(context.Principal));
                var claims = principal.Claims;
                userContext.Id = long.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
                userContext.Account = claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
                userContext.Name = claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value;
                var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
                userContext.RemoteIpAddress = remoteIpAddress is null ? string.Empty : remoteIpAddress.MapToIPv4().ToString();
                return Task.CompletedTask;
            }
                 ,
            //认证失败时调用
            OnAuthenticationFailed = context =>
            {
                //如果是过期，在http heard中加入act参数
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    context.Response.Headers.Add("act", "expired");
                return Task.CompletedTask;
            }
                ,
            //未授权时调用
            OnChallenge = context => Task.CompletedTask
        };

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
        ,string roleIds
        ,string loginerType)
    {
        if (jti.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(jti));

        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Jti,jti),
            new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName),
            new Claim(JwtRegisteredClaimNames.NameId, nameId),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim(JwtBearerDefaults.RoleIds, roleIds),
            new Claim(JwtBearerDefaults.LoginerType,loginerType)
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
        var parameters = GenarateTokenValidationParameters(jwtConfig);
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
        int expiresMinutes = tokenType.Equals(Tokens.AccessToken) ? jwtConfig.Expire : jwtConfig.RefreshTokenExpire;
        var expires = DateTime.Now.AddMinutes(expiresMinutes);
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
