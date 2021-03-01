using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Adnc.Core.Shared.Entities;

namespace Adnc.Core.Shared.IRepositories
{
    /// <summary>
    /// Ef默认的、全功能的仓储接口
    /// 适合传统三层模式开发，实体必须继承 EfEntity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEfRepository<TEntity> : IEfBaseRepository<TEntity>
       where TEntity : EfEntity
    {
        IQueryable<TEntity> GetAll(bool writeDb = false, bool noTracking = true);

        IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false, bool noTracking = true) where TrdEntity : EfEntity;

        Task<TEntity> FindAsync(long keyValue, Expression<Func<TEntity, dynamic>> navigationPropertyPath = null, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default);

        Task<TEntity> FetchAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, dynamic>> navigationPropertyPath = null, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default);

        Task<TResult> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default);

        Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default);

        Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync(long keyValue, CancellationToken cancellationToken = default);

        Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);
    }
}