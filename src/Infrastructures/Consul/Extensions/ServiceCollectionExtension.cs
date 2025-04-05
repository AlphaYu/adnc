using Adnc.Infra.Consul.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraConsul(this IServiceCollection services, IConfigurationSection consulSection, Action<ConsulClientConfiguration>? configOverride = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(consulSection, nameof(consulSection));

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
                    if (configOverride is null)
                    {
                        x.Address = new Uri(configOptions.Value.ConsulUrl);
                        x.Token = configOptions.Value.Token;
                    }
                    else
                    {
                        configOverride.Invoke(x);
                    }
                });
            });
    }
}
