namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEfCoreMySql(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddAdncInfraEfCoreMySql)))
            return services;

        services.TryAddScoped<UnitOfWorkStatus>();
        services.TryAddScoped<IUnitOfWork, UnitOfWork<AdncDbContext>>();
        services.TryAddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
        services.TryAddScoped(typeof(IEfBasicRepository<>), typeof(EfBasicRepository<>));
        return services;
    }
}