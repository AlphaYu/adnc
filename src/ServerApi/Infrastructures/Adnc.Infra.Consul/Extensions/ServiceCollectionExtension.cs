﻿using Adnc.Infra.Consul.Registrar;

namespace Microsoft.Extensions.DependencyInjection;

public static class AdncInfraConsulServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraConsul(this IServiceCollection services, IConfigurationSection consulSection)
        => AddAdncInfraConsul(services, consulSection.Get<ConsulConfig>());

    public static IServiceCollection AddAdncInfraConsul(this IServiceCollection services, ConsulConfig consulConfig)
    {
        services.AddScoped<ITokenGenerator, DefaultTokenGenerator>();
        services.AddScoped<SimpleDiscoveryDelegatingHandler>();
        services.AddScoped<ConsulDiscoverDelegatingHandler>();
        services.AddScoped<ConsulWithGrpcDiscoverDelegatingHandler>();
        services.AddSingleton(x => new ConsulClient(x => x.Address = new Uri(consulConfig.ConsulUrl)));
        services.AddSingleton<ConsulRegistration>();
        return services;
    }
}