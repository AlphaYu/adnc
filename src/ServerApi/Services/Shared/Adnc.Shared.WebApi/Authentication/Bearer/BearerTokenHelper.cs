namespace Adnc.Shared.WebApi.Authentication.Bearer;

public static class BearerTokenHelper
{
    public static Claim[] UnPackFromToken(string securityToken)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(securityToken);
        if (token is null || token.Claims.IsNullOrEmpty())
            return default;
        return token.Claims.ToArray();
    }

    public static Func<BearerTokenValidatedContext, Task> TokenValidatedDelegate =>
        (context) =>
        {
            var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
            var claims = context.Principal.Claims;
            userContext.Id = long.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
            userContext.Account = claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
            userContext.Name = claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value;
            userContext.RemoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            return Task.CompletedTask;
        };
}
