namespace Adnc.Shared.Rpc.Handlers.Token;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtTokenGenerator(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public static string Scheme => "Bearer";

    public virtual string? Create()
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        var tokenTxt = token?.Remove(0, 7);
        return tokenTxt;
    }
}