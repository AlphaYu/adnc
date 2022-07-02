namespace Adnc.Usr.WebApi.Authentication;

public class BearerAuthenticationLocalProcessor : AbstractAuthenticationProcessor
{
    private readonly IAccountAppService _accountAppService;

    public BearerAuthenticationLocalProcessor(IAccountAppService accountAppService) => _accountAppService = accountAppService;

    protected override async Task<(string ValidationVersion, int Status)> GetValidatedInfoAsync(long userId)
    {
        var validatedInfo = await _accountAppService.GetUserValidatedInfoAsync(userId);
        if (validatedInfo is null)
            return (null, 0);

        return (validatedInfo.ValidationVersion, validatedInfo.Status);
    }
}
