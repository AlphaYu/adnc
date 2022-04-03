using Adnc.Infra.Consul;
using Adnc.Infra.Consul.Consumer;
using Adnc.Infra.Consul.TokenGenerator;
using Consul;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class AdncInfraConsulServiceCollectionExtension
{
    public static IServiceCollection AddAdncConsul(this IServiceCollection services, IConfigurationSection consulSection)
    {
        services.AddScoped<ITokenGenerator, DefaultTokenGenerator>();
        services.AddScoped<SimpleDiscoveryDelegatingHandler>();
        services.AddScoped<ConsulDiscoverDelegatingHandler>();
        var url = new System.Uri(consulSection.Get<ConsulConfig>().ConsulUrl);
        services.AddSingleton(x => new ConsulClient(x => x.Address = url));
        return services;
    }
}