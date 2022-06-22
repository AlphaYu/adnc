using Adnc.Shared.WebApi.Authentication.Bearer;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class BearerSchemeExtensions
{
    public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder)
    {
        return builder.AddBearer(BearerDefaults.AuthenticationScheme, delegate
        {
        });
    }

    public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder, Action<BearerSchemeOptions> configureOptions)
    {
        return builder.AddBearer(BearerDefaults.AuthenticationScheme, configureOptions);
    }

    public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder, string authenticationScheme, Action<BearerSchemeOptions> configureOptions)
    {
        return builder.AddBearer(authenticationScheme, authenticationScheme, configureOptions);
    }

    public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<BearerSchemeOptions> configureOptions)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<BearerSchemeOptions>, BearerPostConfigureOptions>());
        return builder.AddScheme<BearerSchemeOptions, BearerAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}