using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Core.Shared.Entities;

namespace Adnc.Core.Shared.IRepositories
{
    /// <summary>
    /// Ef简单的、基础的，初级的仓储接口
    /// 适合DDD开发模式,实体必须继承AggregateRoot
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEfBasicRepository<TEntity> : IEfBaseRepository<TEntity>
               where TEntity : AggregateRoot
    {
        Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity> GetAsync(long keyValue, Expression<Func<TEntity, dynamic>> navigationPropertyPath = null, bool writeDb = false, CancellationToken cancellationToken = default);
    }
}
