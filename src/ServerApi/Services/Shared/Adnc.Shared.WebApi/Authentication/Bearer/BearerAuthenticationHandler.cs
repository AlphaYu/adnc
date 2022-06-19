﻿using System.Text.Encodings.Web;

namespace Adnc.Shared.WebApi.Authentication.Bearer;

/// <summary>
/// Bearer验证(认证)服务
/// </summary>
public class BearerAuthenticationHandler : AuthenticationHandler<BearerSchemeOptions>
{
    public BearerAuthenticationHandler(
        IOptionsMonitor<BearerSchemeOptions> options,
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

            var claims = Options.Events.OnTokenValidating.Invoke(token);
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
        Response.Headers.Add("WWW-Authenticate", "Basic realm=\"aspdotnetcore.net\"");
        authResult = AuthenticateResult.Fail("Invalid Authorization Header");
        return await Task.FromResult(authResult);
    }
}