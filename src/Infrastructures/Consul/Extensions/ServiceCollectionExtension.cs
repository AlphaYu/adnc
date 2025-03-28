﻿using Adnc.Infra.Consul.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraConsul(this IServiceCollection services, IConfigurationSection consulSection, Action<ConsulClientConfiguration>? configOverride = null)
    {
        if (services.HasRegistered(nameof(AddAdncInfraConsul)))
        {
            return services;
        }

        return services
            .Configure<ConsulOptions>(consulSection)
            .AddSingleton(provider =>
            {
                var configOptions = provider.GetRequiredService<IOptions<ConsulOptions>>();
                return new ConsulClient(x =>
                {
                    x.Address = new Uri(configOptions.Value.ConsulUrl);
                    x.Token = configOptions.Value.Token;
                    configOverride?.Invoke(x);
                });
            });
    }
}