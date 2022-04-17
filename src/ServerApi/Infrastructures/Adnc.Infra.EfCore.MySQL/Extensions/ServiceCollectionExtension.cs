namespace Microsoft.Extensions.DependencyInjection;

public static class AdncEfCoreMySqlServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEfCoreMySql(this IServiceCollection services)
    {
        services.AddScoped<UnitOfWorkStatus>();
        services.AddScoped<IUnitOfWork, UnitOfWork<AdncDbContext>>();
        services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IEfBasicRepository<>), typeof(EfBasicRepository<>));
        return services;
    }
}