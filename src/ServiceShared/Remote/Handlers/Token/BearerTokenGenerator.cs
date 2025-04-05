namespace Adnc.Shared.Remote.Handlers.Token;

public class BearerTokenGenerator(IHttpContextAccessor httpContextAccessor, ILogger<BearerTokenGenerator> logger) : ITokenGenerator
{
    public static string Scheme => "Bearer";

    public string GeneratorName => Scheme;

    public string Create()
    {
        var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var tokenTxt = token?.Remove(0, 7);
        logger.LogDebug("Accessor:{tokenTxt}", tokenTxt);
        return tokenTxt ?? string.Empty;
    }
}
