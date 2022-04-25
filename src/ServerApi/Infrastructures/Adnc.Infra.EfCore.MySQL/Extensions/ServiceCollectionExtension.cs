namespace Microsoft.Extensions.DependencyInjection;

public static class AdncEfCoreMySqlServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEfCoreMySql(this IServiceCollection services)
    {
        services.TryAddScoped<UnitOfWorkStatus>();
        services.TryAddScoped<IUnitOfWork, UnitOfWork<AdncDbContext>>();
        services.TryAddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
        services.TryAddScoped(typeof(IEfBasicRepository<>), typeof(EfBasicRepository<>));
        return services;
    }
}