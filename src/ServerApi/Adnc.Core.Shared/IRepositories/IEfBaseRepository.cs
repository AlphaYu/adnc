using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Core.Shared.Entities;

namespace Adnc.Core.Shared.IRepositories
{
    /// <summary>
    /// Ef仓储的基类接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEfBaseRepository<TEntity> : IRepository<TEntity>
               where TEntity : Entity, IEfEntity<long>
    {
        Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool writeDb = false, bool noTracking = true);

        Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

        Task<IPagedModel<TEntity>> PagedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, bool writeDb = false, CancellationToken cancellationToken = default);
    }
}
