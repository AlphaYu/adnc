using Adnc.Core.Shared.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Core.Shared.IRepositories
{
    /// <summary>
    /// A MongoDB based repository of <see cref="TEntity" />.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IMongoRepository<TEntity> : IRepository<TEntity>
        where TEntity : MongoEntity
    {
        /// <summary>
        /// Gets the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities in this repository.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces the specified entity with the same identifier.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<TEntity> ReplaceAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<IPagedModel<TEntity>> PagedAsync(int pageNumber, int pageSize, FilterDefinition<TEntity> filter, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, CancellationToken cancellationToken = default);
    }
}