namespace Adnc.Shared.WebApi.Authentication.Processors;

public class BearerAuthenticationRemoteProcessor(IHttpContextAccessor contextAccessor, IAuthRestClient authRestClient, ILogger<BearerAuthenticationRemoteProcessor> logger)
    : AbstractAuthenticationProcessor
{
    protected override async Task<(string? ValidationVersion, bool Status)> GetValidatedInfoAsync(long userId)
    {
        var userContext = contextAccessor?.HttpContext?.RequestServices.GetService<UserContext>();
        if (userContext is null)
        {
            throw new InvalidDataException(nameof(userContext));
        }
        else if (userContext.Id == 0)
        {
            userContext.Id = userId;
        }

        var apiReuslt = await authRestClient.GetValidatedInfoAsync();
        if (!apiReuslt.IsSuccessStatusCode)
        {
            logger.LogDebug("{StatusCode}:{Message}", apiReuslt.StatusCode, apiReuslt.Error?.Message);
            return (null, false);
        }

        var content = apiReuslt.Content;
        var status = content?.Status ?? false;
        return (content?.ValidationVersion, status);
    }
}
