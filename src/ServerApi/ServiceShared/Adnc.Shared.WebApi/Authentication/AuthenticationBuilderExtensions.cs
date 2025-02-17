using Adnc.Shared.WebApi.Authentication.Basic;
using Adnc.Shared.WebApi.Authentication.Bearer;
using Adnc.Shared.WebApi.Authentication.Hybrid;

namespace Microsoft.Extensions.DependencyInjection.Extensions;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddHybrid(this AuthenticationBuilder builder, Action<HybridSchemeOptions>? configureOptions = null)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<HybridSchemeOptions>, HybridPostConfigureOptions>());
        return builder.AddScheme<HybridSchemeOptions, HybridAuthenticationHandler>(HybridDefaults.AuthenticationScheme, HybridDefaults.AuthenticationScheme, configureOptions);
    }

    public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, Action<BasicSchemeOptions>? configureOptions = null)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<BasicSchemeOptions>, BasicPostConfigureOptions>());
        return builder.AddScheme<BasicSchemeOptions, BasicAuthenticationHandler>(BasicDefaults.AuthenticationScheme, BasicDefaults.AuthenticationScheme, configureOptions);
    }

    public static AuthenticationBuilder AddCustomJwtBearer(this AuthenticationBuilder builder, Action<BearerSchemeOptions>? configureOptions = null)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<BearerSchemeOptions>, BearerPostConfigureOptions>());
        return builder.AddScheme<BearerSchemeOptions, BearerAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme, configureOptions);
    }
}
