using Adnc.Infra.IRepositories;
using Adnc.Infra.Repository.Mongo.Configuration;
using Adnc.Infra.Repository.Mongo.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Adnc.Infra.Repository.Mongo.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IServiceCollection"/> to add easy MongoDB wiring.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the MongoDB context with the specified service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        /// <remarks>
        /// This currently requires wiring up memory caching and logging.
        /// </remarks>
        public static MongoConfigurationBuilder AddAdncInfraMongo<TContext>(this IServiceCollection services, Action<MongoRepositoryOptions> configurator)
            where TContext : IMongoContext
        {
            if (services.HasRegistered(nameof(AddAdncInfraMongo)))
                return new MongoConfigurationBuilder(services);

            services.Configure(configurator);
            services.AddSingleton(typeof(IMongoContext), typeof(TContext));
            services.AddTransient(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddTransient(typeof(ISoftDeletableMongoRepository<>), typeof(SoftDeletableMongoRepository<>));
            return new MongoConfigurationBuilder(services);
        }

        /// <summary>
        /// Registers the MongoDB context with the specified service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        /// <remarks>
        /// This currently requires wiring up memory caching and logging.
        /// </remarks>
        public static MongoConfigurationBuilder AddAdncInfraMongo<TContext>(this IServiceCollection services, string connectionString)
             where TContext : IMongoContext
        {
            return services.AddAdncInfraMongo<TContext>(c => c.ConnectionString = connectionString);
        }
    }
}