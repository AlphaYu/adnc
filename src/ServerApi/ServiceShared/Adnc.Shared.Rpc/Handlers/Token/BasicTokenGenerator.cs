namespace Adnc.Shared.Rpc.Handlers.Token;

public class BasicTokenGenerator : ITokenGenerator
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BasicTokenGenerator> _logger;

    public BasicTokenGenerator(
        IHttpContextAccessor httpContextAccessor,
        ILogger<BasicTokenGenerator> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public static string Scheme => "Basic";

    public string GeneratorName => Scheme;

    public virtual string Create()
    {
        var userContext = _httpContextAccessor.HttpContext.RequestServices.GetService<UserContext>();
        long userId = userContext is null ? 0 : userContext.Id;
        _logger.LogDebug($"UserContext:{userId}");
        var userName = $"{BasicTokenValidator.InternalCaller}-{userId}";
        var token = BasicTokenValidator.PackToBase64(userName);
        return token;
    }
}