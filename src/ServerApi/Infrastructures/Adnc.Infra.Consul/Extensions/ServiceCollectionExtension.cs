using Adnc.Infra.Consul.Consumer;
using Adnc.Infra.Consul.TokenGenerator;
using Consul;

namespace Microsoft.Extensions.DependencyInjection;

public static class AdncInfraConsulServiceCollectionExtension
{
    public static IServiceCollection AddAdncConsul(this IServiceCollection services, string consulAddress)
    {
        services.AddScoped<ITokenGenerator, DefaultTokenGenerator>();
        services.AddScoped<SimpleDiscoveryDelegatingHandler>();
        services.AddScoped<ConsulDiscoverDelegatingHandler>();
        services.AddSingleton(x => new ConsulClient(cfg => cfg.Address = new System.Uri(consulAddress)));
        return services;
    }
}