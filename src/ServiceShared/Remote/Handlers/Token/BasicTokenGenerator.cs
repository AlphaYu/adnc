using Microsoft.Extensions.Options;

namespace Adnc.Shared.Remote.Handlers.Token;

public class BasicTokenGenerator(IHttpContextAccessor httpContextAccessor, IOptions<BasicOptions> basicOptions, ILogger<BasicTokenGenerator> logger) : ITokenGenerator
{
    public static string Scheme => "Basic";

    public string GeneratorName => Scheme;

    public virtual string Create()
    {
        var userContext = httpContextAccessor.HttpContext.RequestServices.GetService<UserContext>();
        var userId = userContext is null ? 0 : userContext.Id;
        logger.LogDebug("UserContext:{userId}", userId);
        var token = BasicTokenValidator.PackToBase64(userId, basicOptions.Value);
        return token;
    }
}
