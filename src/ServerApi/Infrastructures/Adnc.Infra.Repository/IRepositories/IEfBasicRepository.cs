using Adnc.Infra.Entities;

namespace Adnc.Infra.IRepositories;

/// <summary>
/// Ef简单的、基础的，初级的仓储接口
/// 适合DDD开发模式,实体必须继承AggregateRoot
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEfBasicRepository<TEntity> : IEfBaseRepository<TEntity>
           where TEntity : Entity, IEfEntity<long>
{
    Task<int> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity?> GetAsync(long keyValue, Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, bool writeDb = false, CancellationToken cancellationToken = default);
}