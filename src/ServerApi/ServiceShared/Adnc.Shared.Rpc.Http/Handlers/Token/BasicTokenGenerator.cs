namespace Adnc.Shared.Rpc.Http.Handlers.Token;

public class BasicTokenGenerator : ITokenGenerator
{
    private readonly UserContext? _userContext;
    private readonly ILogger<BasicTokenGenerator> _logger;

    public BasicTokenGenerator(
        IHttpContextAccessor httpContextAccessor,
        ILogger<BasicTokenGenerator> logger)
    {
        _userContext = httpContextAccessor.HttpContext.RequestServices.GetService<UserContext>();
        _logger = logger;
    }

    public static string Scheme => "Basic";

    public string GeneratorName => Scheme;

    public virtual string Create()
    {
        long userId = _userContext is null ? 0 : _userContext.Id;
        _logger.LogDebug($"UserContext:{userId}");
        var userName = $"{BasicTokenValidator.InternalCaller}-{userId}";
        var token = BasicTokenValidator.PackToBase64(userName);
        return token;
    }
}