using Adnc.Shared.WebApi.Authentication.Basic;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class BasicSchemeExtensions
{
    public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder)
    {
        return builder.AddBasic(BasicDefaults.AuthenticationScheme, delegate
        {
        });
    }

    public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, Action<BasicSchemeOptions> configureOptions)
    {
        return builder.AddBasic(BasicDefaults.AuthenticationScheme, configureOptions);
    }

    public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme, Action<BasicSchemeOptions> configureOptions)
    {
        return builder.AddBasic(authenticationScheme, authenticationScheme, configureOptions);
    }

    public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<BasicSchemeOptions> configureOptions)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<BasicSchemeOptions>, BasicPostConfigureOptions>());
        return builder.AddScheme<BasicSchemeOptions, BasicAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}