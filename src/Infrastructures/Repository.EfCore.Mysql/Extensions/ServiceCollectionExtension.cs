using System.Reflection;
using Adnc.Infra.Repository.EfCore.MySql;
using Adnc.Infra.Repository.EfCore.MySql.Transaction;
using Adnc.Infra.Repository.Interceptor.Castle;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

        services.AddDbContext<DbContext, MySqlDbContext>(optionsBuilder, serviceLifetime);
        services.Add(new ServiceDescriptor(typeof(IUnitOfWork), typeof(MySqlUnitOfWork<MySqlDbContext>), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(UowInterceptor), typeof(UowInterceptor), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(UowAsyncInterceptor), typeof(UowAsyncInterceptor), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(IEfRepository<>), typeof(EfRepository<>), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(IEfBasicRepository<>), typeof(EfBasicRepository<>), serviceLifetime));

        if (assembly is not null)
        {
            var serviceType = typeof(IEntityInfo);
            var implType = assembly.ExportedTypes.SingleOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
            if (implType is not null)
            {
                services.TryAdd(new ServiceDescriptor(serviceType, implType, ServiceLifetime.Singleton));
            }
        }

        return services;
    }
}
