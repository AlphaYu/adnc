﻿namespace Adnc.Shared.WebApi.Authentication;

public class BearerAuthenticationRemoteProcessor : AbstractAuthenticationProcessor
{
    private IHttpContextAccessor _contextAccessor;
    private IAuthRestClient _authRestClient;
    private ILogger<BearerAuthenticationRemoteProcessor> _logger;

    public BearerAuthenticationRemoteProcessor(
        IHttpContextAccessor contextAccessor,
        IAuthRestClient authRestClient,
        ILogger<BearerAuthenticationRemoteProcessor> logger
        )
    {
        _contextAccessor = contextAccessor;
        _authRestClient = authRestClient;
        _logger = logger;
    }

    protected override async Task<(string? ValidationVersion, int Status)> GetValidatedInfoAsync(long userId)
    {
        var userContext = _contextAccessor?.HttpContext?.RequestServices.GetService<UserContext>();
        if (userContext is null)
            throw new NullReferenceException(nameof(userContext));
        else if (userContext.Id == 0)
            userContext.Id = userId;

        var apiReuslt = await _authRestClient.GetValidatedInfoAsync();
        if (!apiReuslt.IsSuccessStatusCode)
        {
            var message = $"{apiReuslt.StatusCode}:{apiReuslt.Error?.Message}";
            _logger.LogDebug(message);
            return (null, 0);
        }

        return (apiReuslt?.Content?.ValidationVersion, apiReuslt.Content.Status);
    }
}
