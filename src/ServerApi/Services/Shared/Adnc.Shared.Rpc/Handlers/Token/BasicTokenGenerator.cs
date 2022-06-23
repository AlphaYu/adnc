namespace Adnc.Shared.Rpc.Handlers.Token;

public class BasicTokenGenerator : ITokenGenerator
{
    private readonly UserContext? _userContext;
    public BasicTokenGenerator(IHttpContextAccessor httpContextAccessor)
    {
        _userContext = httpContextAccessor.HttpContext.RequestServices.GetService<UserContext>();
    }

    public static string Scheme => "Basic";

    public virtual string Create()
    {
        long userId;
        if (_userContext is null)
            userId = 0;
        else if (_userContext.Id == 0)
            userId = _userContext.ExationId;
        else
            userId = _userContext.Id;

        var userName = $"{BasicTokenValidator.InternalCaller}-{userId}";
        var token = BasicTokenValidator.PackToBase64(userName);
        return token;
    }
}