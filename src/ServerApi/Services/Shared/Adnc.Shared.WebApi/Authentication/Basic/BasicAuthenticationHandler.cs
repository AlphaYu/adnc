using Adnc.Shared.Application.Contracts;
using System.Text.Encodings.Web;
using Adnc.Shared.Rpc.Handlers.Token;


namespace Microsoft.AspNetCore.Authentication.Basic;

/// <summary>
/// Basic验证(认证)服务
/// </summary>
public class BasicAuthenticationHandler : AuthenticationHandler<BasicSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<BasicSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
        ) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult authResult;
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader != null && authHeader.StartsWith(BasicDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
        {
            var startIndex = BasicDefaults.AuthenticationScheme.Length+1;
            var token = authHeader[startIndex..].Trim();
            var (isSuccessful, userName, appId) = BasicTokenGeneratorExtension.UnPackFromBase64(null, token);
            if (isSuccessful)
            {
                var claims = new[] { new Claim("name", userName), new Claim(ClaimTypes.Role, "partner") };
                var identity = new ClaimsIdentity(claims, BasicDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                authResult = AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, BasicDefaults.AuthenticationScheme));

                var userContext = Context.RequestServices.GetService<UserContext>();
                userContext.Id = appId.ToLong().Value;
                userContext.Account = userName;
                userContext.Name = userName;
                userContext.RemoteIpAddress = Context.Connection.RemoteIpAddress.MapToIPv4().ToString();

                return await Task.FromResult(authResult);
            }

            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            authResult = AuthenticateResult.Fail("Invalid Authorization Token");
            return await Task.FromResult(authResult);
        }
        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        Response.Headers.Add("WWW-Authenticate", "Basic realm=\"aspdotnetcore.net\"");
        authResult = AuthenticateResult.Fail("Invalid Authorization Header");
        return await Task.FromResult(authResult);
    }
}