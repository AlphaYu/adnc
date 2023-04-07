using Adnc.Infra.Entities;
using Adnc.Infra.Repository.Mongo.Configuration;
using Adnc.Infra.Repository.Mongo.Entities;
using MongoDB.Driver;

namespace Adnc.Infra.Repository.Mongo.Extensions
{
    /// <summary>
    /// Extensions for <see cref="MongoIndexContext{TEntity}"/>.
    /// </summary>
    public static class MongoIndexContextExtensions
    {
        /// <summary>
        /// Adds the specified named index.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="optionsConfigurator">The options configurator.</param>
        public static void Add<TEntity>(
            this MongoIndexContext<TEntity> context,
            string name,
            IndexKeysDefinition<TEntity> keys,
            Action<CreateIndexOptions>? optionsConfigurator = null)
            where TEntity : MongoEntity
        {
            var options = new CreateIndexOptions { Name = name };
            optionsConfigurator?.Invoke(options);
            context.Add(new CreateIndexModel<TEntity>(keys, options));
        }

        /// <summary>
        /// Adds the date_deleted index that's required for soft deletable support.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        public static void AddSoftDeletableIndex<TEntity>(this MongoIndexContext<TEntity> context)
            where TEntity : SoftDeletableMongoEntity
        {
            context.Add("date_deleted",
                Builders<TEntity>.IndexKeys.Ascending(x => x.DateDeleted));
        }

        /// <summary>
        /// Adds the date_created index that's required for expiring entity support.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="expireAfter">The expire after.</param>
        public static void AddExpiringIndex<TEntity>(this MongoIndexContext<TEntity> context, TimeSpan expireAfter)
            where TEntity : ExpiringMongoEntity
        {
            context.Add("date_created",
                Builders<TEntity>.IndexKeys.Ascending(x => x.DateCreated),
                o => o.ExpireAfter(expireAfter));
        }
    }
}