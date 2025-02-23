namespace Adnc.Infra.Repository;

public interface IMongoDbRepository<TEntity> : IEfBaseRepository<TEntity>
    where TEntity : Entity, IEfEntity<long>
{
    Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TResult?> FetchAsync<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, object>>? orderByExpression = null, bool ascending = false, CancellationToken cancellationToken = default);
}