using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Authentication.Hybrid;

/// <summary>
/// Hybrid验证(认证)服务
/// </summary>
public sealed class HybridAuthenticationHandler : AuthenticationHandler<HybridSchemeOptions>
{
    public HybridAuthenticationHandler(IOptionsMonitor<HybridSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            return await Task.FromResult(AuthenticateResult.NoResult());

        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader.IsNotNullOrWhiteSpace())
        {
            if (authHeader.StartsWith(JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                return await Context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            if (authHeader.StartsWith(BasicDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                return await Context.AuthenticateAsync(BasicDefaults.AuthenticationScheme);
        }

        Response.StatusCode = (int)HttpStatusCode.Forbidden;
        return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
    }
}