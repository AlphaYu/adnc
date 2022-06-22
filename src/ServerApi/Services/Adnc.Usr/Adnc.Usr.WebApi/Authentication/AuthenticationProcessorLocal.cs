namespace Adnc.Usr.WebApi.Authentication;

public class AuthenticationProcessorLocal : AbstracAuthenticationProcessor
{
    private readonly IAccountAppService _accountAppService;

    public AuthenticationProcessorLocal(IAccountAppService accountAppService) => _accountAppService = accountAppService;

    protected override async Task<(string ValidationVersion, int Status)> GetValidatedInfoAsync(long userId)
    {
        var validatedInfo = await _accountAppService.GetUserValidatedInfoAsync(userId);
        if (validatedInfo is null)
            return (null, 0);

        return (validatedInfo.ValidationVersion, validatedInfo.Status);
    }
}
