using Adnc.Shared.WebApi.Authentication.Hybrid;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class HybridSchemeExtensions
{
    public static AuthenticationBuilder AddHybrid(this AuthenticationBuilder builder)
    {
        return builder.AddHybrid(HybridDefaults.AuthenticationScheme, delegate
        {
        });
    }

    public static AuthenticationBuilder AddHybrid(this AuthenticationBuilder builder, Action<HybridSchemeOptions> configureOptions)
    {
        return builder.AddHybrid(HybridDefaults.AuthenticationScheme, configureOptions);
    }

    public static AuthenticationBuilder AddHybrid(this AuthenticationBuilder builder, string authenticationScheme, Action<HybridSchemeOptions> configureOptions)
    {
        return builder.AddHybrid(authenticationScheme, authenticationScheme, configureOptions);
    }

    public static AuthenticationBuilder AddHybrid(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<HybridSchemeOptions> configureOptions)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<HybridSchemeOptions>, HybridPostConfigureOptions>());
        return builder.AddScheme<HybridSchemeOptions, HybridAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}