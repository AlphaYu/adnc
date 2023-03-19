namespace Adnc.Shared.WebApi.Authentication.Hybrid;

/// <summary>
/// Hybrid验证(认证)服务
/// </summary>
public sealed class HybridAuthenticationHandler : AuthenticationHandler<HybridSchemeOptions>
{
    private ILogger<HybridAuthenticationHandler> _logeer;
    public HybridAuthenticationHandler(IOptionsMonitor<HybridSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _logeer = logger.CreateLogger<HybridAuthenticationHandler>();
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        var requestId = System.Diagnostics.Activity.Current?.Id ?? Context.TraceIdentifier;
        Logger.LogDebug($"requestid: {requestId}");

        if (endpoint is null)
            return await Task.FromResult(AuthenticateResult.NoResult());

        if (endpoint.Metadata.GetMetadata<IAllowAnonymous>() is not null)
            return await Task.FromResult(AuthenticateResult.NoResult());

        var authHeader = Request.Headers["Authorization"].ToString();
        _logeer.LogDebug($"Authorization: {authHeader}");
        if (authHeader.IsNotNullOrWhiteSpace())
        {
            var scheme = authHeader.Split(" ")[0];
            return await Context.AuthenticateAsync(scheme);
        }

        Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
    }
}