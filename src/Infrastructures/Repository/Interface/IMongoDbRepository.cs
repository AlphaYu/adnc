namespace Adnc.Infra.Repository;

public interface IMongoDbRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity<string>
{
    Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool noTracking = true);

    Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity?> FetchAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>>? orderByExpression = null, bool ascending = false, CancellationToken cancellationToken = default);
}
