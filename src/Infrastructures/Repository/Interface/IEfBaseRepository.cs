namespace Adnc.Infra.Repository;

/// <summary>
/// Base interface for EF repositories
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEfBaseRepository<TEntity> : IRepository<TEntity>
           where TEntity : Entity, IEfEntity<long>
{
    /// <summary>
    /// Inserts a single entity.
    /// </summary>
    /// <param name="entity"><see cref="T:TEntity"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts a batch of entities.
    /// </summary>
    /// <param name="entities"><see cref="T:TEntity"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a single entity.
    /// </summary>
    /// <param name="entity"><see cref="T:TEntity"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a batch of entities.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether any entity matches the given condition.
    /// </summary>
    /// <param name="whereExpression">Query condition</param>
    /// <param name="writeDb">Whether to use the read-write database; default false (optional)</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts entities matching the given condition.
    /// </summary>
    /// <param name="whereExpression">Query condition</param>
    /// <param name="writeDb">Whether to use the read-write database; default false (optional)</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Queries by condition and returns IQueryable{TEntity}.
    /// </summary>
    /// <param name="expression">Query condition</param>
    /// <param name="writeDb">Whether to use the read-write database; default false (optional)</param>
    /// <param name="noTracking">Whether to disable tracking; default false (optional)</param>
    /// <returns></returns>
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool writeDb = false, bool noTracking = true);
}
