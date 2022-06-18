namespace Adnc.Shared.WebApi.Authentication.Basic;

public static class BasicTokenHelper
{
    public static Func<BasicTokenValidatedContext, Task> TokenValidatedDelegate =>
        (context) =>
        {
            var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
            var claims = context.Principal.Claims;
            userContext.Id = long.Parse(claims.First(x => x.Type == BasicDefaults.NameId).Value);
            userContext.Account = claims.First(x => x.Type == BasicDefaults.UniqueName).Value;
            userContext.Name = claims.First(x => x.Type == BasicDefaults.Name).Value;
            userContext.RemoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            return Task.CompletedTask;
        };
}
