using Adnc.Infra.Repository.Mongo.Entities;
using Adnc.Infra.Repository.Mongo.Extensions;
using Adnc.Infra.Repository.Mongo.Interfaces;
using MongoDB.Driver;

namespace Adnc.Infra.Repository.Mongo
{
    /// <summary>
    /// A MongoDB based repository of <see cref="T:TEntity"/> that supports soft deletion.
    /// Entities that implement soft deletion should probably have an index defined on the <see cref="SoftDeletableMongoEntity.DateDeleted"/> field.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="MongoRepository{TEntity}" />
    public class SoftDeletableMongoRepository<TEntity> : MongoRepository<TEntity>, ISoftDeletableMongoRepository<TEntity>
        where TEntity : SoftDeletableMongoEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoftDeletableMongoRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SoftDeletableMongoRepository(IMongoContext context) : base(context)
        {
        }

        /// <summary>
        /// Soft deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var deleted = await UpdateAsync(id, Builders<TEntity>.Update.Set(x => x.DateDeleted, DateTime.UtcNow), null, cancellationToken);
            return deleted?.Id == id;
        }

        /// <summary>
        /// Un-deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task<bool> UnDeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var deleted = await UpdateAsync(id, Builders<TEntity>.Update.Unset(x => x.DateDeleted), null, cancellationToken);
            return deleted?.Id == id;
        }

        /// <summary>
        /// Gets the non-deleted entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default) =>
            await FindOneAsync(Filter.NotDeletedAndIdEq(id), null, cancellationToken);

        /// <summary>
        /// Gets all non-deleted entities in this repository.
        /// </summary>
        /// <returns></returns>
        public override async Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await FindAsync(Filter.NotDeleted(), null, cancellationToken);
    }
}