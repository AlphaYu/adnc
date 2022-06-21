namespace Adnc.Shared.WebApi.Authentication.Bearer;

/// <summary>
/// Bearer验证(认证)服务
/// </summary>
public class BearerAuthenticationHandler : AuthenticationHandler<BearerSchemeOptions>
{
    private IAuthentication _authentication;

    public BearerAuthenticationHandler(
        IOptionsMonitor<BearerSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IAuthentication authentication
        ) : base(options, logger, encoder, clock)
    {
        _authentication = authentication;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult authResult;
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader is not null && authHeader.StartsWith(BearerDefaults.AuthenticationScheme))
        {
            var startIndex = BearerDefaults.AuthenticationScheme.Length + 1;
            var token = authHeader[startIndex..].Trim();
            if (token.IsNullOrWhiteSpace())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                authResult = AuthenticateResult.Fail("Invalid Authorization Token");
                return await Task.FromResult(authResult);
            }

            var claims = await _authentication.ValidateAsync(token);

            if (claims.IsNotNullOrEmpty())
            {
                var identity = new ClaimsIdentity(claims, BearerDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                authResult = AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, BearerDefaults.AuthenticationScheme));
                var validatedContext = new BearerTokenValidatedContext(Context, Scheme, Options)
                {
                    Principal = claimsPrincipal
                };
                await Options.Events.OnTokenValidated.Invoke(validatedContext);
                return await Task.FromResult(authResult);
            }

            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            authResult = AuthenticateResult.Fail("Invalid Authorization Token");
            return await Task.FromResult(authResult);
        }
        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //Response.Headers.Add("WWW-Authenticate", "Basic realm=\"aspdotnetcore.net\"");
        authResult = AuthenticateResult.Fail("Invalid Authorization Header");
        return await Task.FromResult(authResult);
    }
}