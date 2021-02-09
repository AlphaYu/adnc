using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Adnc.Core.Shared.Entities;
using System.Collections.Generic;
using System.Data;

namespace Adnc.Core.Shared.IRepositories
{
    public interface IEfRepository<TEntity> : IRepository<TEntity>
       where TEntity : EfEntity
    {
        IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false, bool noTracking = true) where TrdEntity : EfEntity;

        Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default);

        Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default);

        Task<TResult> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true,CancellationToken cancellationToken = default);

        Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync(long keyValue, CancellationToken cancellationToken = default);

        IQueryable<TEntity> GetAll(bool writeDb = false, bool noTracking = true);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool writeDb = false, bool noTracking = true);

        Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);

        Task<TEntity> FindAsync(long keyValue, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default);

        Task<IPagedModel<TEntity>> PagedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, bool writeDb = false, CancellationToken cancellationToken = default);

    }
}