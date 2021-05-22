using Adnc.Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Core.Shared.IRepositories
{
    /// <summary>
    /// Ef仓储的基类接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEfBaseRepository<TEntity> : IRepository<TEntity>
               where TEntity : Entity, IEfEntity<long>
    {
        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entities"><see cref="TEntity"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件查询实体是否存在
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="writeDb">是否读写库，默认false,可选参数</param>
        /// param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 统计符合条件的实体数量
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="writeDb">是否读写库，默认false,可选参数</param>
        /// param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件查询，返回IQueryable<TEntity>
        /// </summary>
        /// <param name="expression">查询条件</param>
        /// <param name="writeDb">是否读写库，默认false,可选参数</param>
        /// <param name="noTracking">是否开启跟踪，默认false,可选参数</param>
        /// <returns></returns>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool writeDb = false, bool noTracking = true);

        /// <summary>
        /// Dapper查询
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">commandTimeout</param>
        /// <param name="commandType">commandType</param>
        /// <param name="writeDb">是否读写库，默认false,可选参数</param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

        /// <summary>
        /// Dapper查询
        /// </summary>
        /// <typeparam name="TResult"><see cref="TResult"/></typeparam>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">commandTimeout</param>
        /// <param name="commandType">commandType</param>
        /// <param name="writeDb">是否读写库，默认false,可选参数</param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="pageNumber">第几页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="whereExpression">查询条件</param>
        /// <param name="orderByExpression">排序条件</param>
        /// <param name="ascending">排序方式</param>
        /// <param name="writeDb">是否读写库，默认false,可选参数</param>
        /// param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns></returns>
        Task<IPagedModel<TEntity>> PagedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, bool writeDb = false, CancellationToken cancellationToken = default);
    }
}