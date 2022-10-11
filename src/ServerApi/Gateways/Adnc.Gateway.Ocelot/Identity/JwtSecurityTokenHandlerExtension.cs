using Adnc.Infra.Core.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Adnc.Gateway.Ocelot.Identity;

public static class JwtSecurityTokenHandlerExtension
{
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
}
