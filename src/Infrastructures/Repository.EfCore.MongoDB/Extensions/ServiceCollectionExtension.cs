using System.Reflection;
using Adnc.Infra.Repository.EfCore.MongoDB;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEfCoreMongoDb(this IServiceCollection services, Assembly assembly, Action<DbContextOptionsBuilder> optionsBuilder, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));
        ArgumentNullException.ThrowIfNull(optionsBuilder, nameof(optionsBuilder));

        if (services.HasRegistered(nameof(AddAdncInfraEfCoreMongoDb)))
        {
            return services;
        }

        services.AddDbContext<DbContext, MongoDbContext>(optionsBuilder, serviceLifetime);
        services.Add(new ServiceDescriptor(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>), serviceLifetime));

        var serviceType = typeof(IEntityInfo);
        var implType = assembly.ExportedTypes.Single(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
        services.Add(new ServiceDescriptor(serviceType, implType, serviceLifetime));

        return services;
    }
}
