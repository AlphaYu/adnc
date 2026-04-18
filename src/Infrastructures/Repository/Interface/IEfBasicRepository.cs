namespace Adnc.Infra.Repository;

/// <summary>
/// Simple, basic EF repository interface.
/// Suitable for DDD development; entity must inherit AggregateRoot.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEfBasicRepository<TEntity> : IEfBaseRepository<TEntity>
           where TEntity : Entity, IEfEntity<long>
{
    Task<int> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity?> GetAsync(long keyValue, Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, bool writeDb = false, CancellationToken cancellationToken = default);

    Task<TEntity> GetRequiredAsync(long keyValue, Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, bool writeDb = false, CancellationToken cancellationToken = default);
}
