using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Z.EntityFramework.Plus;
using Dapper;
using Adnc.Infr.Common.Extensions;
using Adnc.Core.Shared.IRepositories;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared;
using Adnc.Infr.EfCore.Extensions;

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

        public virtual IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false) where TrdEntity : EfEntity
        {
            var dbSet = DbContext.Set<TrdEntity>().AsNoTracking();
            if (writeDb)
                return dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            return dbSet;
        }

        public virtual IQueryable<TEntity> GetAll(bool writeDb = false)
        {
            
            var dbSet= DbContext.Set<TEntity>().AsNoTracking();
            if(writeDb)
                return dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            return dbSet;
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

        public async virtual Task<int> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] propertyExpressions)
        {
            if (propertyExpressions == null || propertyExpressions.Length == 0)
            {
                DbContext.Update(entity);
            }
            else
            {
                var entry = DbContext.GetEntityEntry(entity, out var existBefore);

                if (existBefore)
                {
                    var propNames = propertyExpressions.Select(x => x.GetMemberName()).ToArray();

                    foreach (var propEntry in entry.Properties)
                    {
                        if (!propNames.Contains(propEntry.Metadata.Name))
                        {
                            propEntry.IsModified = false;
                        }
                    }
                }
                else
                {
                    entry.State = EntityState.Unchanged;
                    foreach (var expression in propertyExpressions)
                    {
                        entry.Property(expression).IsModified = true;
                    }
                }
            }
            return await DbContext.SaveChangesAsync(default);
        }

        public virtual async Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression,Expression<Func<TEntity, TEntity>> upDateExpression, CancellationToken cancellationToken = default)
        {
            var mapping = DbContext.Model.FindEntityType(typeof(TEntity));
            var property = mapping.GetProperties().Where(p => p.Name == "RowVersion").FirstOrDefault();

            if (property != null)
            {
                throw new Exception("该实体有RowVersion列，不能使用批量更新");
            }

            

            return await DbContext.Set<TEntity>().Where(whereExpression).UpdateAsync(upDateExpression,cancellationToken);
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default)
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

        public virtual async Task<TEntity> FindAsync(long keyValue, bool writeDb = false,CancellationToken cancellationToken = default)
        {
            var dbSet = DbContext.Set<TEntity>().AsNoTracking();
            if (writeDb)
                dbSet = dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            return await dbSet.Where(t => t.ID == keyValue).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false,CancellationToken cancellationToken = default)
        {
            dynamic result;

            var dbSet = DbContext.Set<TEntity>().AsNoTracking();
            if (writeDb)
                dbSet = dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);

            var query = dbSet.Where(whereExpression);

            if (orderByExpression == null)
            {
                result = await query.Select(selector).FirstOrDefaultAsync();
            }
            else
            {
                result = ascending
                          ? await query.OrderBy(orderByExpression).Select(selector).FirstOrDefaultAsync()
                          : await query.OrderByDescending(orderByExpression).Select(selector).FirstOrDefaultAsync()
                          ;
            }

            if (result == null)
                return null;

            return (typeof(TEntity) == typeof(TResult))
                ? await Task.FromResult(result as TEntity)
                : await Task.FromResult(JsonSerializer.Deserialize<TEntity>(JsonSerializer.Serialize(result)))
                ;
        }

        public virtual async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false,CancellationToken cancellationToken = default)
        {
            var dbSet = DbContext.Set<TEntity>().AsNoTracking();
            if (writeDb)
                dbSet = dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);

            var query = dbSet.Where(whereExpression);
            if (orderByExpression != null)
            {
                if (ascending)
                {
                    query = query.OrderBy(orderByExpression);
                }
                else
                {
                    query = query.OrderByDescending(orderByExpression);
                }
            }

            return await query.Select(selector).ToListAsync();
        }

        public virtual async Task<List<TResult>> SelectAsync<TResult>(int count, Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression=null, bool ascending = false, bool writeDb = false, CancellationToken cancellationToken = default)
        {
            var dbSet = DbContext.Set<TEntity>().AsNoTracking();
            if (writeDb)
                dbSet = dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);

            var query = dbSet.Where(whereExpression);
            if (orderByExpression != null)
            {
                if (ascending)
                {
                    query = query.OrderBy(orderByExpression);
                }
                else
                {
                    query = query.OrderByDescending(orderByExpression);
                }
            }

            return await query.Select(selector).Take(count).ToListAsync(cancellationToken);
        }

        public virtual async Task<IPagedModel<TEntity>> PagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, bool writeDb = false,CancellationToken cancellationToken = default)
        {
            var dbSet = DbContext.Set<TEntity>().AsNoTracking();
            if (writeDb)
                dbSet = dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);

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

        protected EntityEntry<TEntity> Entry(TEntity entity)
        {
            return DbContext.Entry<TEntity>(entity);
        }
    }
}
