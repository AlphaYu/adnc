namespace Adnc.Shared.WebApi.Authentication.JwtBearer;

public static class JwtSecurityTokenHandlerExtension
{
    public static TokenValidationParameters GenarateTokenValidationParameters(JwtConfig tokenConfig) =>
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

    public static JwtBearerEvents GenarateJwtBearerEvents() =>
        new()
        {
            //接受到消息时调用
            OnMessageReceived = context => Task.CompletedTask
                ,
            //在Token验证通过后调用
            OnTokenValidated = context =>
            {
                var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
                var claims = context.Principal.Claims;
                userContext.Id = long.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
                userContext.Account = claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
                userContext.Name = claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value;
                userContext.RemoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
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
}
