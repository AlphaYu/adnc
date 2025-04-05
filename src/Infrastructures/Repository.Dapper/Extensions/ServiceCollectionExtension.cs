using Adnc.Infra.Repository.Dapper;
using Adnc.Infra.Repository.Dapper.Internal;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraDapper(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        if (services.HasRegistered(nameof(AddAdncInfraDapper)))
        {
            return services;
        }

        services.Add(new ServiceDescriptor(typeof(IAdoExecuterWithQuerierRepository), typeof(DapperRepository), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(IAdoExecuterRepository), typeof(DapperRepository), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(IAdoQuerierRepository), typeof(DapperRepository), serviceLifetime));
        SqlMapper.AddTypeHandler(new TimeStampeHandler());
        //services.TryAddScoped<IAdoExecuterRepository>(provider => provider.GetRequiredService<IAdoExecuterWithQuerierRepository>());
        //services.TryAddScoped<IAdoQuerierRepository>(provider => provider.GetRequiredService<IAdoExecuterWithQuerierRepository>());
        return services;
    }
}
