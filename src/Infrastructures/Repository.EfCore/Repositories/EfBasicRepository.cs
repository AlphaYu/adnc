namespace Adnc.Infra.Repository.EfCore;

/// <summary>
/// Ef简单的、基础的，初级的仓储接口
/// 适合DDD开发模式,实体必须继承AggregateRoot
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EfBasicRepository<TEntity>(DbContext dbContext) : AbstractEfBaseRepository<DbContext, TEntity>(dbContext), IEfBasicRepository<TEntity>
        where TEntity : Entity, IEfEntity<long>
{
    public virtual async Task<int> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Remove(entity);
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        DbContext.RemoveRange(entities);
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetAsync(long keyValue, Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, bool writeDb = false, CancellationToken cancellationToken = default)
    {
        var query = GetDbSet(writeDb, false).Where(t => t.Id == keyValue);
        if (navigationPropertyPath is null)
        {
            return await query.FirstOrDefaultAsync(cancellationToken);
        }
        else
        {
            return await query.Include(navigationPropertyPath).FirstOrDefaultAsync(cancellationToken);
        }
    }

    public virtual async Task<TEntity> GetRequiredAsync(long keyValue, Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, bool writeDb = false, CancellationToken cancellationToken = default)
    {
        var entity = await GetAsync(keyValue, navigationPropertyPath, writeDb, cancellationToken);
        return entity is null ? throw new InvalidDataException($"The entity with id {keyValue} was not found.") : entity;
    }

    public virtual async Task<TEntity?> GetAsync(long keyValue, IEnumerable<Expression<Func<TEntity, dynamic>>>? navigationPropertyPaths = null, bool writeDb = false, CancellationToken cancellationToken = default)
    {
        if (navigationPropertyPaths is null)
        {
            return await GetAsync(keyValue, navigationPropertyPath: null, writeDb, cancellationToken);
        }

        if (navigationPropertyPaths.Count() == 1)
        {
            return await GetAsync(keyValue, navigationPropertyPaths.First(), writeDb, cancellationToken);
        }

        var query = GetDbSet(writeDb, false).Where(t => t.Id == keyValue);
        foreach (var navigationPath in navigationPropertyPaths)
        {
            query = query.Include(navigationPath);
        }
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}