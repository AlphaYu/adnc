namespace Adnc.Shared.WebApi.Authentication;

public class BearerAuthenticationRemoteProcessor : AbstractAuthenticationProcessor
{
    private IHttpContextAccessor _contextAccessor;
    private IAuthRestClient _authRestClient;

    public BearerAuthenticationRemoteProcessor(
        IHttpContextAccessor contextAccessor,
        IAuthRestClient authRestClient
        )
    {
        _contextAccessor = contextAccessor;
        _authRestClient = authRestClient;
    }

    protected override async Task<(string ValidationVersion, int Status)> GetValidatedInfoAsync(long userId)
    {
        var userContext = _contextAccessor.HttpContext.RequestServices.GetService<UserContext>();
        userContext.ExationId = userId;

        var apiReuslt = await _authRestClient.GetValidatedInfoAsync();
        if (!apiReuslt.IsSuccessStatusCode)
            return (null, 0);

        return (apiReuslt.Content.ValidationVersion, apiReuslt.Content.Status);
    }
}
