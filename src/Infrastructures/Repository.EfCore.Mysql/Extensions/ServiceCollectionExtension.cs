using System.Reflection;
using Adnc.Infra.Repository.EfCore.MySql;
using Adnc.Infra.Repository.EfCore.MySql.Transaction;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEfCoreMySql(this IServiceCollection services, Assembly? assembly, Action<DbContextOptionsBuilder> optionsBuilder, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(optionsBuilder, nameof(optionsBuilder));

        if (services.HasRegistered(nameof(AddAdncInfraEfCoreMySql)))
        {
            return services;
        }
        services.AddDbContext<DbContext, MySqlDbContext>(optionsBuilder, serviceLifetime, ServiceLifetime.Singleton);
        services.Add(new ServiceDescriptor(typeof(IUnitOfWork), typeof(MySqlUnitOfWork<MySqlDbContext>), serviceLifetime));
        services.AddUowInterceptor(serviceLifetime);
        services.AddEfRepository(serviceLifetime);
        services.AddEntityInfo(assembly);
        return services;
    }
}
