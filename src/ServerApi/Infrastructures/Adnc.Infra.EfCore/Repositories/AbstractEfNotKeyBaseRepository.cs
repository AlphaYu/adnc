namespace Adnc.Infra.Repository.EfCore.Repositories;
/// <summary>
/// Ef仓储的基类实现,抽象类
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public abstract class AbstractEfNotKeyBaseRepository<TDbContext, TEntity> : IEfNotKeyBaseRepository<TEntity>
   where TDbContext : DbContext
   where TEntity : EntityNotKey, IEntityNotKey
{
    protected virtual TDbContext DbContext { get; }

    protected AbstractEfNotKeyBaseRepository(TDbContext dbContext) => DbContext = dbContext;

    protected virtual IQueryable<TEntity> GetDbSet(bool writeDb, bool noTracking)
    {
        if (noTracking && writeDb)
            return DbContext.Set<TEntity>().AsNoTracking().TagWith(RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        else if (noTracking)
            return DbContext.Set<TEntity>().AsNoTracking();
        else if (writeDb)
            return DbContext.Set<TEntity>().TagWith(RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        else
            return DbContext.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool writeDb = false, bool noTracking = true)
    {
        return this.GetDbSet(writeDb, noTracking).Where(expression);
    }

    public virtual async Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(entities);
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default)
    {
        var dbSet = DbContext.Set<TEntity>().AsNoTracking();
        if (writeDb)
            dbSet = dbSet.TagWith(RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        return await EntityFrameworkQueryableExtensions.AnyAsync(dbSet, whereExpression, cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default)
    {
        var dbSet = DbContext.Set<TEntity>().AsNoTracking();
        if (writeDb)
            dbSet = dbSet.TagWith(RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        return await EntityFrameworkQueryableExtensions.CountAsync(dbSet, whereExpression);
    }

    public virtual Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {

        //获取实体状态
        var entry = DbContext.Entry(entity);

        //如果实体没有被跟踪，必须指定需要更新的列
        if (entry.State == EntityState.Detached)
            throw new ArgumentException($"实体没有被跟踪，需要指定更新的列");
        //实体被标记为Added或者Deleted，抛出异常，应该不会出现这种状态。
        if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
            throw new ArgumentException($"{nameof(entity)},实体状态为{nameof(entry.State)}");

        return this.UpdateInternalAsync(entity, cancellationToken);
    }

    protected virtual async Task<int> UpdateInternalAsync(TEntity entity, CancellationToken cancellationToken = default)
     => await DbContext.SaveChangesAsync(cancellationToken);

}