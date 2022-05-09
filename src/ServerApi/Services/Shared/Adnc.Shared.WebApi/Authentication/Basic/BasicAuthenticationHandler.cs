using Adnc.Shared.Application.Contracts;
using System.Text.Encodings.Web;

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
            var token = authHeader.Substring($"{BasicDefaults.AuthenticationScheme} ".Length).Trim();
            if (UnpackFromBase64(token, out string userName, out string appId))
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

    public static Dictionary<string, (string partnerId, string securityKey)> Partners => new()
    {
        { "usr", ("10001", "8tf9u") },
        { "maint", ("10002", "66tyr") },
        { "cus", ("10003", "87ygt") },
        { "ord", ("10004", "gh765") },
        { "whse", ("10005", "oi987") }
    };

    public static bool UnpackFromBase64(string base64Token, out string userName, out string appId)
    {
        appId = string.Empty;
        var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(base64Token));
        var credentials = credentialstring.Split(':');
        var unPackUserName = userName = credentials[0];
        var unPackPassword = credentials[1].Split('_');

        if (!Partners.ContainsKey(unPackUserName))
            return false;

        var (partnerId, securityKey) = Partners[unPackUserName];

        var unPackAppId = appId = unPackPassword[0];
        if (!partnerId.EqualsIgnoreCase(appId))
            return false;

        var unPackTotalSeconds = unPackPassword[1].ToLong();
        var currentTotalSeconds = DateTime.Now.GetTotalSeconds();
        var differTotalSeconds = currentTotalSeconds - unPackTotalSeconds;
        if (differTotalSeconds > 120 || differTotalSeconds < -3)
            return false;

        var unPackTicket = unPackPassword[2];
        var validationTicket = InfraHelper.Security.MD5($"{unPackUserName}_{unPackAppId}_{unPackTotalSeconds}_{securityKey}", true);
        return validationTicket.EqualsIgnoreCase(unPackTicket);
    }

    public static string PackToBase64(string userName)
    {
        if (!Partners.ContainsKey(userName))
            throw new ArgumentNullException(nameof(userName));
        var (appId, securityKey) = Partners[userName];

        var currentTotalSeconds = DateTime.Now.GetTotalSeconds();
        var md5String = InfraHelper.Security.MD5($"{userName}_{appId}_{currentTotalSeconds}_{securityKey}", true);
        var plainToken = $"{userName}:{appId}_{currentTotalSeconds}_{md5String}";
        byte[] byteToken = Encoding.UTF8.GetBytes(plainToken);
        return Convert.ToBase64String(byteToken);
    }
}