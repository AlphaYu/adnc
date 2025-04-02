using System.Reflection;
using Adnc.Infra.Repository.EfCore.MongoDB;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEfCoreMongoDb(this IServiceCollection services, Assembly? assembly, Action<DbContextOptionsBuilder> optionsBuilder, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(optionsBuilder, nameof(optionsBuilder));

        if (services.HasRegistered(nameof(AddAdncInfraEfCoreMongoDb)))
        {
            return services;
        }

        services.AddDbContext<DbContext, MongoDbContext>(optionsBuilder, serviceLifetime);
        services.Add(new ServiceDescriptor(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>), serviceLifetime));

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
