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
                throw new ArgumentException($"Entity is not being tracked; this bulk-update method cannot be used.");
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
            {
                throw new ArgumentException($"{nameof(entity)}, entity state is {nameof(entry.State)}");
            }
        }

        return UpdateInternalAsync(cancellationToken);
    }

    public virtual Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        // Get entity state
        var entry = DbContext.Entry(entity);

        // Entity is not tracked
        if (entry.State == EntityState.Detached)
        {
            throw new ArgumentException($"Entity is not being tracked.");
        }

        // Entity is Added or Deleted — should never happen in ADNC, but guard anyway
        if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
        {
            throw new ArgumentException($"{nameof(entity)}, entity state is {nameof(entry.State)}");
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
