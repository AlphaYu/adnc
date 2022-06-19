using Adnc.Infra.Entities;

namespace Adnc.Infra.Repository.Mongo.Configuration
{
    /// <summary>
    /// Mongo entity builder.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public sealed class MongoEntityBuilder<TEntity>
        where TEntity : MongoEntity
    {
        /// <summary>
        /// Configures indexes.
        /// </summary>
        public MongoIndexContext<TEntity> Indexes { get; } = new MongoIndexContext<TEntity>();

        /// <summary>
        /// Configures seed data.
        /// </summary>
        public IList<TEntity> Seed { get; } = new List<TEntity>();
    }
}