namespace Adnc.Shared.WebApi.Authentication;

public class ValidationInfo
{
    public long Id { get; set; } 
    public string? ValidationVersion { get; set; }
    public int Status { get; set; } 
}

public class BearerAuthenticationCacheProcessor : AbstractAuthenticationProcessor
{
    private IHttpContextAccessor _contextAccessor;
    private ICacheProvider _cacheProvider;
    private ILogger<BearerAuthenticationCacheProcessor> _logger;

    public BearerAuthenticationCacheProcessor(
        IHttpContextAccessor contextAccessor,
        ICacheProvider cacheProvider,
        ILogger<BearerAuthenticationCacheProcessor> logger
        )
    {
        _contextAccessor = contextAccessor;
        _cacheProvider = cacheProvider;
        _logger = logger;
    }

    protected override async Task<(string? ValidationVersion, int Status)> GetValidatedInfoAsync(long userId)
    {
        var userContext = _contextAccessor?.HttpContext?.RequestServices.GetService<UserContext>();

        if (userContext is null)
            throw new NullReferenceException(nameof(userContext));
        else if (userContext.Id == 0)
            userContext.Id = userId;

        var cacheKey = $"{CacheConsts.UserValidatedInfoKeyPrefix}:{userId}";
        var validationInfo = await _cacheProvider.GetAsync<ValidationInfo>(cacheKey);

        if (validationInfo == null || validationInfo.Value == null)
        {
            _logger.LogDebug($"cacheValue [{cacheKey}] is null");
            return (null, 0);
        }

        return (validationInfo.Value.ValidationVersion, validationInfo.Value.Status);
    }
}
