namespace Microsoft.Extensions.DependencyInjection;

public static class AdncInfraDapperServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraDapper(this IServiceCollection services)
    {
        services.TryAddScoped<DapperRepository>();
        services.TryAddScoped<IAdoExecuterRepository>(provider => provider.GetRequiredService<DapperRepository>());
        services.TryAddScoped<IAdoQuerierRepository>(provider => provider.GetRequiredService<DapperRepository>());
        return services;
    }
}