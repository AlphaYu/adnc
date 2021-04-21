using System;
using Microsoft.Extensions.DependencyInjection;
using Adnc.Infra.Mongo.Configuration;
using Adnc.Infra.Mongo.Interfaces;
using Adnc.Core.Shared.IRepositories;

namespace Adnc.Infra.Mongo.Extensions
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
        public static MongoConfigurationBuilder AddMongo<TContext>(this IServiceCollection services, Action<MongoRepositoryOptions> configurator)
            where TContext: IMongoContext
        {
            services.Configure(configurator);
            services.AddSingleton(typeof(IMongoContext), typeof(TContext));
            //services.AddTransient(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            //services.AddTransient(typeof(ISoftDeletableMongoRepository<>), typeof(SoftDeletableMongoRepository<>));
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
        public static MongoConfigurationBuilder AddMongo<TContext>(this IServiceCollection services, string connectionString)
             where TContext : IMongoContext
        {
            return services.AddMongo<TContext>(c => c.ConnectionString = connectionString);
        }
    }
}
