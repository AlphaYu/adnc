namespace Adnc.Shared.WebApi.Authentication.Processors;

public class ValidationInfo
{
    public long Id { get; set; }
    public string? ValidationVersion { get; set; }
    public bool Status { get; set; }
}

public class BearerAuthenticationCacheProcessor(IHttpContextAccessor contextAccessor, ICacheProvider cacheProvider, ILogger<BearerAuthenticationCacheProcessor> logger)
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

        var cacheKey = $"{GeneralConsts.UserValidatedInfoKeyPrefix}:{userId}";
        var validationInfo = await cacheProvider.GetAsync<ValidationInfo>(cacheKey);

        if (validationInfo == null || validationInfo.Value == null)
        {
            logger.LogDebug("cacheValue [{cacheKey}] is null,", cacheKey);
            return (null, false);
        }

        return (validationInfo.Value.ValidationVersion, validationInfo.Value.Status);
    }
}
