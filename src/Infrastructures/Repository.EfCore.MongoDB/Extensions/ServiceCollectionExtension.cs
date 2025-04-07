using System.Reflection;
using Adnc.Infra.Repository.EfCore.MongoDB;

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

        services.AddDbContext<DbContext, MongoDbContext>(optionsBuilder, serviceLifetime, ServiceLifetime.Singleton);
        services.Add(new ServiceDescriptor(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>), serviceLifetime));
        services.AddEntityInfo(assembly);
        return services;
    }
}
