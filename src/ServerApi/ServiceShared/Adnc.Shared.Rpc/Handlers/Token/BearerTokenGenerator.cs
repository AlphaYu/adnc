namespace Adnc.Shared.Rpc.Handlers.Token;

public class BearerTokenGenerator : ITokenGenerator
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BearerTokenGenerator> _logger;

    public BearerTokenGenerator(
        IHttpContextAccessor httpContextAccessor,
         ILogger<BearerTokenGenerator> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public static string Scheme => "Bearer";

    public string GeneratorName => Scheme;

    public string Create()
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var tokenTxt = token?.Remove(0, 7);
        _logger.LogDebug($"Accessor:{tokenTxt}");
        return tokenTxt ?? string.Empty;
    }
}