namespace Adnc.Infra.Repository.EfCore.MongoDB;

/// <summary>
///   mongodb repository implement
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class MongoDbRepository<TEntity>(DbContext dbContext) : IMongoDbRepository<TEntity>
        where TEntity : MongoEntity
{
    protected virtual DbContext DbContext { get; } = dbContext;

    public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool noTracking = true)
        => GetDbSet(noTracking).Where(expression);

    public virtual async Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
        => await DbContext.Set<TEntity>().AsNoTracking().AnyAsync(whereExpression, cancellationToken);

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
        => await DbContext.Set<TEntity>().AsNoTracking().CountAsync(whereExpression, cancellationToken);

    public virtual Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            var entry = DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                throw new ArgumentException($"实体没有被跟踪，不能使用该批量更新方法");
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
            {
                throw new ArgumentException($"{nameof(entity)},实体状态为{nameof(entry.State)}");
            }
        }

        return UpdateInternalAsync(cancellationToken);
    }

    public virtual Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        //获取实体状态
        var entry = DbContext.Entry(entity);

        //如果实体没有被跟踪，必须指定需要更新的列
        if (entry.State == EntityState.Detached)
        {
            throw new ArgumentException($"实体没有被跟踪");
        }

        //实体被标记为Added或者Deleted，抛出异常，ADNC应该不会出现这种状态。
        if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
        {
            throw new ArgumentException($"{nameof(entity)},实体状态为{nameof(entry.State)}");
        }

        return UpdateInternalAsync(cancellationToken);
    }

    public virtual async Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Remove(entity);
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        DbContext.RemoveRange(entities);
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> FetchAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>>? orderByExpression = null, bool ascending = false, CancellationToken cancellationToken = default)
    {
        var query = GetDbSet(false).Where(whereExpression);
        if (orderByExpression != null)
        {
            query = ascending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
        }
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    protected virtual IQueryable<TEntity> GetDbSet(bool noTracking)
    {
        if (noTracking)
        {
            return DbContext.Set<TEntity>().AsNoTracking();
        }
        else
        {
            return DbContext.Set<TEntity>();
        }
    }

    protected async Task<int> UpdateInternalAsync(CancellationToken cancellationToken = default) =>
        await DbContext.SaveChangesAsync(cancellationToken);
}
