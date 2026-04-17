using Adnc.Shared.WebApi.Authentication.Processors;

namespace Adnc.Demo.Admin.Api.Authentication;

[Obsolete($"use {nameof(BearerAuthenticationCacheProcessor)} instead 2025-02-17")]
public class BearerAuthenticationLocalProcessor(IUserService userAppService) : AbstractAuthenticationProcessor
{
    protected override async Task<(string? ValidationVersion, bool Status)> GetValidatedInfoAsync(long userId)
    {
        var validatedInfo = await userAppService.GetUserValidatedInfoAsync(userId);
        if (validatedInfo is null)
        {
            return (null, false);
        }

        return (validatedInfo.ValidationVersion, validatedInfo.Status);
    }
}
