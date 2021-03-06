using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Adnc.Core.Shared;
using Adnc.Core.Shared.IRepositories;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.Domain.Entities;

namespace Adnc.Infr.EfCore.Repositories
{
    /// <summary>
    /// Ef仓储的基类实现,抽象类
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AbstractEfBaseRepository<TDbContext, TEntity> : IEfBaseRepository<TEntity>
       where TDbContext : DbContext
       where TEntity : Entity, IEfEntity<long>
    {
        protected virtual TDbContext DbContext { get; }

        public AbstractEfBaseRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected virtual IQueryable<TEntity> GetDbSet(bool writeDb, bool noTracking)
        {
            if (noTracking && writeDb)
                return DbContext.Set<TEntity>().AsNoTracking().TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            else if (noTracking && writeDb == false)
                return DbContext.Set<TEntity>().AsNoTracking();
            else if (noTracking == false && writeDb)
                return DbContext.Set<TEntity>().TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            else
                return DbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool writeDb = false, bool noTracking = true)
        {
            return this.GetDbSet(writeDb, noTracking).Where(expression);
        }

        public virtual async Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
        {
            if (writeDb)
                sql = string.Concat("/* ", EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
            return await DbContext.Database.GetDbConnection().QueryAsync(sql, param, null, commandTimeout, commandType);
        }

        public virtual async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
        {
            if (writeDb)
                sql = string.Concat("/* ", EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
            return await DbContext.Database.GetDbConnection().QueryAsync<TResult>(sql, param, null, commandTimeout, commandType);
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
                dbSet = dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            return await dbSet.AnyAsync(whereExpression, cancellationToken);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default)
        {
            var dbSet = DbContext.Set<TEntity>().AsNoTracking();
            if (writeDb)
                dbSet = dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            return await dbSet.CountAsync(whereExpression);
        }

        public virtual async Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            //获取实体状态
            var entry = DbContext.Entry(entity);

            //如果实体没有被跟踪，必须指定需要更新的列
            if (entry.State == EntityState.Detached)
                throw new ArgumentException($"实体没有被跟踪，需要指定更新的列");

            //实体没有被更改
            if (entry.State == EntityState.Unchanged)
            {
                var navigations = entry.Navigations.Where(x => x.CurrentValue is ValueObject);
                if (navigations?.Count() > 0)
                {
                    foreach (var navigation in navigations)
                    {
                        DbContext.Add(navigation.CurrentValue);
                    }
                }
                else
                    return await Task.FromResult(0);
            }

            //实体被标记为Added或者Deleted，抛出异常。
            //ADNC应该不会出现这种状态
            if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
                throw new ArgumentException($"{nameof(entity)},实体状态为{nameof(entry.State)}");

            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IPagedModel<TEntity>> PagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, bool writeDb = false, CancellationToken cancellationToken = default)
        {
            var dbSet = this.GetDbSet(writeDb, false);

            var total = await dbSet.CountAsync(whereExpression, cancellationToken);
            if (total == 0)
            {
                return new PagedModel<TEntity>() { PageSize = pageSize };
            }

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            var query = dbSet.Where(whereExpression);
            query = ascending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
            var data = await EntityFrameworkQueryableExtensions.ToArrayAsync(
                                    query.Skip((pageIndex - 1) * pageSize).Take(pageSize)
                                   , cancellationToken);

            return new PagedModel<TEntity>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
                Data = data
            };
        }
    }
}
