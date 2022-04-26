namespace Microsoft.Extensions.DependencyInjection;

public static class AdncInfraDapperServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraDapper(this IServiceCollection services)
    {
        services.TryAddScoped<IAdoExecuterWithQuerierRepository, DapperRepository>();
        services.TryAddScoped<IAdoExecuterRepository>(provider => provider.GetRequiredService<IAdoExecuterWithQuerierRepository>());
        services.TryAddScoped<IAdoQuerierRepository>(provider => provider.GetRequiredService<IAdoExecuterWithQuerierRepository>());
        return services;
    }
}