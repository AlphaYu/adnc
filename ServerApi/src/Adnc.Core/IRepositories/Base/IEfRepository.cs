using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Common.Models;
using Adnc.Core.Entities;

namespace Adnc.Core.IRepositories
{
    public interface IEfRepository<TEntity> : IRepository<TEntity>
       where TEntity : EfEntity
    {
        DbSet<TEntity> GetAll();

        Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync<TKeyType>(TKeyType[] keyValues, CancellationToken cancellationToken = default);

        Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        Task<int> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] propertyExpressions);

        Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> upDateExpression, CancellationToken cancellationToken = default);

        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);

        Task<TEntity> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, CancellationToken cancellationToken = default);

        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, CancellationToken cancellationToken = default);

        Task<List<TResult>> SelectAsync<TResult>(int count, Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, CancellationToken cancellationToken = default);

        Task<IPagedModel<TEntity>> PagedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, CancellationToken cancellationToken = default);

        EntityEntry<TEntity> Entry(TEntity entity);
    }
}
