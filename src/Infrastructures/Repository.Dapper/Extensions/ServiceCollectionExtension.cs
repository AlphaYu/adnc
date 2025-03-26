using Adnc.Infra.Repository.Dapper;
using Adnc.Infra.Repository.Dapper.Internal;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraDapper(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddAdncInfraDapper)))
        {
            return services;
        }

        services.TryAddScoped<IAdoExecuterWithQuerierRepository, DapperRepository>();
        services.TryAddScoped<IAdoExecuterRepository, DapperRepository>();
        services.TryAddScoped<IAdoQuerierRepository, DapperRepository>();
        SqlMapper.AddTypeHandler(new TimeStampeHandler());
        //services.TryAddScoped<IAdoExecuterRepository>(provider => provider.GetRequiredService<IAdoExecuterWithQuerierRepository>());
        //services.TryAddScoped<IAdoQuerierRepository>(provider => provider.GetRequiredService<IAdoExecuterWithQuerierRepository>());
        return services;
    }
}