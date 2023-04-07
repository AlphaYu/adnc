using Adnc.Infra.Entities;
using MongoDB.Driver;

namespace Adnc.Infra.IRepositories;

/// <summary>
/// A MongoDB based repository of <see cref="T:TEntity" />.
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
    /// Gets the entity with the specified identifier.
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity> GetAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default);

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
    /// Batch delete
    /// </summary>
    /// <param name="filter">删除条件</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<long> DeleteManyAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the specified entity with the same identifier.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<TEntity> ReplaceAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="filter"></param>
    /// <param name="orderByExpression"></param>
    /// <param name="ascending"></param>
    /// <param name="cancellationToken"></param>
    Task<PagedModel<TEntity>> PagedAsync(int pageNumber, int pageSize, FilterDefinition<TEntity> filter, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, CancellationToken cancellationToken = default);
}