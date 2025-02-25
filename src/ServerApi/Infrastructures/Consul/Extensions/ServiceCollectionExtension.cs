using Adnc.Infra.Consul.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraConsul(this IServiceCollection services, IConfigurationSection consulSection)
    {
        if (services.HasRegistered(nameof(AddAdncInfraConsul)))
            return services;

        return services
            .Configure<ConsulOptions>(consulSection)
            .AddSingleton(provider =>
            {
                var configOptions = provider.GetService<IOptions<ConsulOptions>>();
                if (configOptions is null)
                    throw new NullReferenceException(nameof(configOptions));
                return new ConsulClient(x => x.Address = new Uri(configOptions.Value.ConsulUrl));
            })
            ;
    }
}