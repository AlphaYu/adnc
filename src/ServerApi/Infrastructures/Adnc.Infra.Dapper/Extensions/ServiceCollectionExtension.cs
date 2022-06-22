using Adnc.Infra.Dapper.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraDapper(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddAdncInfraDapper)))
            return services;

        services.TryAddScoped<IAdoExecuterWithQuerierRepository, DapperRepository>();
        services.TryAddScoped<IAdoExecuterRepository, DapperRepository>();
        services.TryAddScoped<IAdoQuerierRepository, DapperRepository>();
        //services.TryAddScoped<IAdoExecuterRepository>(provider => provider.GetRequiredService<IAdoExecuterWithQuerierRepository>());
        //services.TryAddScoped<IAdoQuerierRepository>(provider => provider.GetRequiredService<IAdoExecuterWithQuerierRepository>());
        return services;
    }
}