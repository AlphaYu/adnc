using Adnc.Infra.Repository.EfCore.MySQL.Transaction;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEfCoreMySql(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsBuilder)
    {
        if (services.HasRegistered(nameof(AddAdncInfraEfCoreMySql)))
            return services;

        services.TryAddScoped<IUnitOfWork,MySQLUnitOfWork<MySQLDbContext>>();
        services.TryAddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
        services.TryAddScoped(typeof(IEfBasicRepository<>), typeof(EfBasicRepository<>));
        services.AddDbContext<DbContext,MySQLDbContext>(optionsBuilder);

        return services;
    }
}