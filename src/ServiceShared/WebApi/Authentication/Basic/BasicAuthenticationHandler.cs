namespace Adnc.Shared.WebApi.Authentication.Basic;

/// <summary>
/// Basic验证(认证)服务
/// </summary>
public class BasicAuthenticationHandler(IOptionsMonitor<BasicSchemeOptions> options, IOptions<BasicOptions> basicOptions, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<BasicSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult authResult;
        var authHeader = Request.Headers.Authorization.ToString();
        if (authHeader is not null && authHeader.StartsWith(BasicDefaults.AuthenticationScheme))
        {
            var startIndex = BasicDefaults.AuthenticationScheme.Length + 1;
            var token = authHeader[startIndex..].Trim();
            if (token.IsNullOrWhiteSpace())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                authResult = AuthenticateResult.Fail("Invalid Authorization Token");
                return await Task.FromResult(authResult);
            }

            var validatedResult = BasicTokenValidator.UnPackFromBase64(token, basicOptions.Value);
            if (validatedResult is not null && validatedResult.IsSuccessful)
            {
                if (string.IsNullOrWhiteSpace(validatedResult.UserName))
                {
                    throw new ArgumentNullException(nameof(validatedResult.UserName));
                }

                if (validatedResult.UserId is null)
                {
                    throw new ArgumentNullException(nameof(validatedResult.UserId));
                }

                var userId = validatedResult.UserId.ToString() ?? "0";
                var claims = new[] {
                        new Claim(BasicDefaults.NameId, userId)
                        , new Claim(BasicDefaults.UniqueName, validatedResult.UserName)
                        , new Claim(BasicDefaults.Name, validatedResult.UserName)
                };
                var identity = new ClaimsIdentity(claims, BasicDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                authResult = AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, BasicDefaults.AuthenticationScheme));
                var validatedContext = new BasicTokenValidatedContext(Context, Scheme, Options)
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
        else
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //Response.Headers.Add("WWW-Authenticate", "Basic realm=\"aspdotnetcore.net\"");
            authResult = AuthenticateResult.Fail("Invalid Authorization Header");
            return await Task.FromResult(authResult);
        }
    }
}
