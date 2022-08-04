namespace Adnc.Infra.Repository.EfCore.Repositories;
/// <summary>
/// Ef简单的、基础的，初级的仓储接口
/// 适合DDD开发模式,实体必须继承AggregateRoot
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EfNotKeyBasicRepository<TEntity> : AbstractEfNotKeyBaseRepository<DbContext, TEntity>, IEfNotKeyBasicRepository<TEntity>
        where TEntity : EntityNotKey, IEntityNotKey
{
    public EfNotKeyBasicRepository(DbContext dbContext)
        : base(dbContext)
    {
    }

    public virtual async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        this.DbContext.UpdateRange(entities);
        return await this.DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.DbContext.Remove(entity);
        return await this.DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        this.DbContext.RemoveRange(entities);
        return await this.DbContext.SaveChangesAsync(cancellationToken);
    }
}
