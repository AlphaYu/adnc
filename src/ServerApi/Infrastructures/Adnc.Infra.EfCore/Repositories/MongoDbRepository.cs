namespace Adnc.Infra.Repository.EfCore;

/// <summary>
///   mongodb repository implement
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class MongoDbRepository<TEntity>(DbContext dbContext) : AbstractEfBaseRepository<DbContext, TEntity>(dbContext), IMongoDbRepository<TEntity>
        where TEntity : Entity, IEfEntity<long>
{
    public virtual async Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.DbContext.Remove(entity);
        return await this.DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        this.DbContext.RemoveRange(entities);
        return await this.DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TResult?> FetchAsync<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, object>>? orderByExpression = null, bool ascending = false, CancellationToken cancellationToken = default)
    {
        var query = this.GetDbSet(false, false).Where(whereExpression);
        if (orderByExpression != null)
        {
            query = ascending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
        }
        return await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
    }
}