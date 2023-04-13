namespace Adnc.Demo.Usr.Api.Authentication;

public class BearerAuthenticationLocalProcessor : AbstractAuthenticationProcessor
{
    private readonly IUserAppService _userAppService;

    public BearerAuthenticationLocalProcessor(IUserAppService userAppService) => _userAppService = userAppService;

    protected override async Task<(string? ValidationVersion, int Status)> GetValidatedInfoAsync(long userId)
    {
        var validatedInfo = await _userAppService.GetUserValidatedInfoAsync(userId);
        if (validatedInfo is null)
            return (null, 0);

        return (validatedInfo.ValidationVersion, validatedInfo.Status);
    }
}
