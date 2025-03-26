using Adnc.Infra.Repository.EfCore.MongoDB;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEfCoreMongoDb(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsBuilder)
    {
        if (services.HasRegistered(nameof(AddAdncInfraEfCoreMongoDb)))
        {
            return services;
        }

        //services.TryAddScoped<IUnitOfWork, MongoDbUnitOfWork<MongoDbContext>>();
        services.TryAddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));
        services.AddDbContext<DbContext, MongoDbContext>(optionsBuilder);

        return services;
    }
}