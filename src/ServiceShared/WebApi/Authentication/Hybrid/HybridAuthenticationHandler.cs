namespace Adnc.Shared.WebApi.Authentication.Hybrid;

/// <summary>
/// Hybrid验证(认证)服务
/// </summary>
public sealed class HybridAuthenticationHandler(IOptionsMonitor<HybridSchemeOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder)
    : AuthenticationHandler<HybridSchemeOptions>(options, loggerFactory, encoder)
{
    private readonly ILogger<HybridAuthenticationHandler> _logeer = loggerFactory.CreateLogger<HybridAuthenticationHandler>();

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        var requestId = System.Diagnostics.Activity.Current?.Id ?? Context.TraceIdentifier;
        Logger.LogDebug("requestid: {requestId}", requestId);

        if (endpoint is null)
        {
            return await Task.FromResult(AuthenticateResult.NoResult());
        }

        if (endpoint.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            return await Task.FromResult(AuthenticateResult.NoResult());
        }

        var authHeader = Request.Headers.Authorization.ToString();

        _logeer.LogDebug("{Authorization}: {authHeader}", nameof(Request.Headers.Authorization), authHeader);

        if (authHeader.IsNotNullOrWhiteSpace())
        {
            var scheme = authHeader.Split(" ")[0];
            return await Context.AuthenticateAsync(scheme);
        }

        var accessToken = Context.Request.Query["access_token"];
        if (accessToken.IsNotNullOrEmpty())
        {
            var scheme = JwtBearerDefaults.AuthenticationScheme;
            Request.Headers.Authorization = new Microsoft.Extensions.Primitives.StringValues($"{scheme} {accessToken}");
            return await Context.AuthenticateAsync(scheme);
        }

        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
    }
}
