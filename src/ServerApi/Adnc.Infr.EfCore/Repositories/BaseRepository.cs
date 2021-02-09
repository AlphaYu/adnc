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
using Adnc.Infr.Common.Extensions;
using Z.EntityFramework.Plus;

namespace Adnc.Infr.EfCore.Repositories
{
    public abstract class BaseRepository<TDbContext, TEntity> : IEfRepository<TEntity>
       where TDbContext : DbContext
       where TEntity : EfEntity
    {
        protected virtual TDbContext DbContext { get; }

        public BaseRepository(TDbContext dbContext)
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

        public virtual IQueryable<TEntity> GetAll(bool writeDb = false, bool noTracking = true)
        {
            return this.GetDbSet(writeDb, noTracking);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool writeDb = false, bool noTracking = true)
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

        public virtual async Task<int> DeleteAsync(long keyValue, CancellationToken cancellationToken = default)
        {
            //获取实体是否被跟踪
            var entity = DbContext.Set<TEntity>().Local.FirstOrDefault(x => x.Id == keyValue);

            //如果实体被跟踪，调用Ef原生方法删除
            if (entity != null)
            {
                DbContext.Remove(entity);
                return await DbContext.SaveChangesAsync();
            }

            var mapping = DbContext.Model.FindEntityType(typeof(TEntity)); //3.0
            var properties = mapping.GetProperties();
            var schema = mapping.GetSchema() ?? "dbo";
            var tableName = mapping.GetTableName();
            var keyName = properties.Where(p => p.IsPrimaryKey()).Select(p => p.PropertyInfo.Name).First();
            var isSoftDelete = properties.Where(p => p.Name == "IsDeleted").Any();

            var sql = isSoftDelete
                      ? $"update {tableName} set IsDeleted=true "
                      : $"delete from {tableName} "
                      ;
            var where = $" where {keyName}={keyValue};";

            return await DbContext.Database.ExecuteSqlRawAsync(string.Concat(sql, where), cancellationToken);
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

        public virtual async Task<TEntity> FindAsync(long keyValue, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
        {
            return await this.GetDbSet(writeDb, noTracking).Where(t => t.Id == keyValue).FirstOrDefaultAsync(cancellationToken);
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
                return await Task.FromResult(0);

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
            var data = await query.Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToArrayAsync(cancellationToken);
            return new PagedModel<TEntity>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
                Data = data
            };
        }

        public virtual IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false, bool noTracking = true)
            where TrdEntity : EfEntity
        {
            var queryAble = DbContext.Set<TrdEntity>().AsQueryable();
            if (writeDb)
                queryAble = queryAble.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            if (noTracking)
                queryAble = queryAble.AsNoTracking();
            return queryAble;
        }

        public virtual async Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
        {
            var enityType = typeof(TEntity);
            var isSoftDelete = typeof(ISoftDelete).IsAssignableFrom(enityType);
            if (isSoftDelete)
            {
                var paramExpression = Expression.Parameter(enityType, "e");
                var newExpression = Expression.New(enityType);
                var binding = Expression.Bind(enityType.GetMember("IsDeleted")[0], Expression.Constant(true));
                var memberInitExpression = Expression.MemberInit(newExpression, new List<MemberBinding>() { binding });
                var updateFactory = Expression.Lambda<Func<TEntity, TEntity>>(memberInitExpression, paramExpression);
                return await DbContext.Set<TEntity>().Where(whereExpression).UpdateAsync(updateFactory, cancellationToken);
            }
            return await DbContext.Set<TEntity>().Where(whereExpression).DeleteAsync(cancellationToken);
        }

        public virtual async Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default)
        {
            //没有指定需要更新的列
            if (updatingExpressions?.Length == 0)
                await UpdateAsync(entity, cancellationToken);


            //获取实体状态
            var entry = DbContext.Entry(entity);

            //实体被为没有更改
            if (entry.State == EntityState.Unchanged)
                return await Task.FromResult(0);

            //实体被标记为Added或者Deleted，抛出异常。
            //ADNC应该不会出现这种状态
            if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
                throw new ArgumentException($"{nameof(entity)},实体状态为{nameof(entry.State)}");

            //有指定需要更新的列
            //实体被跟踪
            if (entry.State == EntityState.Modified)
            {
                //备选方案直接抛异常throw new ArgumentException($"{nameof(entity)},实体已经被跟踪,不需要指定更新列");
                var propNames = updatingExpressions.Select(x => x.GetMemberName()).ToArray();
                entry.Properties.ForEach(propEntry =>
                {
                    if (!propNames.Contains(propEntry.Metadata.Name))
                    {
                        propEntry.IsModified = false;
                    }
                });
            }
            //实体没有被跟踪
            else
            {
                var originalEntity = DbContext.Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
                //当前上下文中，没有同Id实体
                if (originalEntity == null)
                {
                    entry.State = EntityState.Unchanged;
                    updatingExpressions.ForEach(expression =>
                    {
                        entry.Property(expression).IsModified = true;
                    });
                }
                else
                {
                    //当前上下文中，有同Id实体
                    var originaEntry = DbContext.Entry(originalEntity);
                    originaEntry.CurrentValues.SetValues(entity);
                    var propNames = updatingExpressions.Select(x => x.GetMemberName()).ToArray();
                    originaEntry.Properties.ForEach(propEntry =>
                    {
                        if (!propNames.Contains(propEntry.Metadata.Name))
                        {
                            propEntry.IsModified = false;
                        }
                    });
                }
            }

            return await DbContext.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default)
        {
            var mapping = DbContext.Model.FindEntityType(typeof(TEntity));
            var property = mapping.GetProperties().Where(p => p.Name == "RowVersion").FirstOrDefault();

            if (property != null)
            {
                throw new Exception("该实体有RowVersion列，不能使用批量更新");
            }
            return await DbContext.Set<TEntity>().Where(whereExpression).UpdateAsync(updatingExpression, cancellationToken);
        }

        public virtual async Task<TResult> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
        {
            TResult result;

            var query = this.GetDbSet(writeDb, noTracking).Where(whereExpression);

            if (orderByExpression == null)
                result = await query.OrderByDescending(x => x.Id).Select(selector).FirstOrDefaultAsync(cancellationToken);
            else
                result = ascending
                          ? await query.OrderBy(orderByExpression).Select(selector).FirstOrDefaultAsync(cancellationToken)
                          : await query.OrderByDescending(orderByExpression).Select(selector).FirstOrDefaultAsync(cancellationToken)
                          ;

            return result;

            //dynamic result;
            //if (result == null)
            //    return null;

            //return (typeof(TEntity) == typeof(TResult))
            //    ? result as TEntity
            //    : JsonSerializer.Deserialize<TEntity>(JsonSerializer.Serialize(result));
            //;
        }

    }
}
