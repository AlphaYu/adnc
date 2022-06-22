using Adnc.Infra.Entities;
using MongoDB.Driver;

namespace Adnc.Infra.Repository.Mongo.Interfaces
{
    /// <summary>
    /// Context used to maintain a single MongoDB connection.
    /// </summary>
    public interface IMongoContext
    {
        /// <summary>
        /// Gets the MongoDB collection for the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IMongoCollection<TEntity>> GetCollectionAsync<TEntity>(CancellationToken cancellationToken = default)
            where TEntity : MongoEntity;

        /// <summary>
        /// Drops the collection for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task DropCollectionAsync<TEntity>(CancellationToken cancellationToken = default);
    }
}