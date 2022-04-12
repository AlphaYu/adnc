namespace Microsoft.Extensions.DependencyInjection;

public static class AdncEfCoreMySqlServiceCollectionExtension
{
    public static IServiceCollection AddAdncEfRepositries(this IServiceCollection services, Assembly entitiesAssemblieToScan)
    {
        services.AddScoped<UnitOfWorkStatus>();
        services.AddScoped<IUnitOfWork, UnitOfWork<AdncDbContext>>();
        services.AddScoped(typeof(EfRepository<>), typeof(IEfRepository<>));
        services.AddScoped(typeof(EfBasicRepository<>), typeof(IEfBasicRepository<>));
        services.Scan(scan => scan.FromAssemblyOf<AdncDbContext>()
                        .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());
        services.Scan(scan => scan.FromAssemblies(entitiesAssemblieToScan)
                        .AddClasses(c => c.AssignableTo<IEntityInfo>())
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());

        return services;
    }
}