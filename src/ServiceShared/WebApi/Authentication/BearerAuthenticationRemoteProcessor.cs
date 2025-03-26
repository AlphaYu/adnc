namespace Adnc.Shared.WebApi.Authentication;

public class BearerAuthenticationRemoteProcessor : AbstractAuthenticationProcessor
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IAuthRestClient _authRestClient;
    private readonly ILogger<BearerAuthenticationRemoteProcessor> _logger;

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

    protected override async Task<(string? ValidationVersion, bool Status)> GetValidatedInfoAsync(long userId)
    {
        var userContext = _contextAccessor?.HttpContext?.RequestServices.GetService<UserContext>();
        if (userContext is null)
        {
            throw new InvalidDataException(nameof(userContext));
        }
        else if (userContext.Id == 0)
        {
            userContext.Id = userId;
        }

        var apiReuslt = await _authRestClient.GetValidatedInfoAsync();
        if (!apiReuslt.IsSuccessStatusCode)
        {
            var message = $"{apiReuslt.StatusCode}:{apiReuslt.Error?.Message}";
            _logger.LogDebug(message);
            return (null, false);
        }

        var content = apiReuslt.Content;
        var status = content?.Status ?? false;
        return (content?.ValidationVersion, status);
    }
}
