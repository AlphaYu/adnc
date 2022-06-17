using Microsoft.Extensions.DependencyInjection;

namespace Adnc.Shared.Rpc.Handlers.Token;

public class BasicTokenGenerator : ITokenGenerator
{
    private readonly UserContext? _userContext;
    public BasicTokenGenerator(IHttpContextAccessor httpContextAccessor)
    {
        _userContext = httpContextAccessor.HttpContext.RequestServices.GetService(typeof(UserContext)) as UserContext;
    }

    public static string Scheme => "Basic";

    public virtual string Create()
    {
        var userName =
            _userContext is null
            ? $"{BasicTokenValidator.InternalCaller}-0"
            : $"{BasicTokenValidator.InternalCaller}-{_userContext.Id}"
            ;
        var token = BasicTokenValidator.PackToBase64(userName);
        return token;
    }
}