namespace Adnc.Demo.Usr.Api.Authentication;

[Obsolete($"use {nameof(BearerAuthenticationCacheProcessor)} instead 2025-02-17")]
public class BearerAuthenticationLocalProcessor : AbstractAuthenticationProcessor
{
    private readonly IUserAppService _userAppService;

    public BearerAuthenticationLocalProcessor(IUserAppService userAppService) => _userAppService = userAppService;

    protected override async Task<(string? ValidationVersion, bool Status)> GetValidatedInfoAsync(long userId)
    {
        var validatedInfo = await _userAppService.GetUserValidatedInfoAsync(userId);
        if (validatedInfo is null)
            return (null, false);

        return (validatedInfo.ValidationVersion, validatedInfo.Status);
    }
}
