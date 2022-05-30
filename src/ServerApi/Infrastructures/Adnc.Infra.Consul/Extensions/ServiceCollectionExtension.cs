using Adnc.Infra.Consul.Discover.Handler;
using Adnc.Infra.Consul.Registrar;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraConsul(this IServiceCollection services, IConfigurationSection consulSection)
        => AddAdncInfraConsul(services, consulSection.Get<ConsulConfig>());

    public static IServiceCollection AddAdncInfraConsul(this IServiceCollection services, ConsulConfig consulConfig)
    {
        if (services.HasRegistered(nameof(AddAdncInfraConsul)))
            return services;

        services.AddScoped<ITokenGenerator, DefaultTokenGenerator>();
        services.AddScoped<SimpleDiscoveryDelegatingHandler>();
        services.AddScoped<ConsulDiscoverDelegatingHandler>();
        services.AddSingleton(x => new ConsulClient(x => x.Address = new Uri(consulConfig.ConsulUrl)));
        services.AddSingleton<ConsulRegistration>();
        services.AddSingleton<IConsulServiceProvider, ConsulServiceProvider>();
        return services;
    }
}