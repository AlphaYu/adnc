using System.Reflection;
using Adnc.Infra.Repository.EfCore.SqlServer;
using Adnc.Infra.Repository.EfCore.SqlServer.Transaction;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEfCoreSQLServer(this IServiceCollection services, Assembly? assembly, Action<DbContextOptionsBuilder> optionsBuilder, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(optionsBuilder, nameof(optionsBuilder));

        if (services.HasRegistered(nameof(AddAdncInfraEfCoreSQLServer)))
        {
            return services;
        }

        services.AddDbContext<DbContext, SqlServerDbContext>(optionsBuilder, serviceLifetime, ServiceLifetime.Singleton);
        services.Add(new ServiceDescriptor(typeof(IUnitOfWork), typeof(SqlServerUnitOfWork<SqlServerDbContext>), serviceLifetime));
        services.AddUowInterceptor(serviceLifetime);
        services.AddEfRepository(serviceLifetime);
        services.AddEntityInfo(assembly);
        return services;
    }
}
