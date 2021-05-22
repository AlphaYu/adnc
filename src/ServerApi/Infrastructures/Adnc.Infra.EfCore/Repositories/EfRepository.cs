using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Adnc.Infra.EfCore.Repositories
{
    /// <summary>
    /// Ef默认的、全功能的仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class EfRepository<TEntity> : AbstractEfBaseRepository<AdncDbContext, TEntity>, IEfRepository<TEntity>
      where TEntity : EfEntity
    {
        public EfRepository(AdncDbContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<TEntity> GetAll(bool writeDb = false, bool noTracking = true)
        {
            return this.GetDbSet(writeDb, noTracking);
        }

        public IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false, bool noTracking = true)
               where TrdEntity : EfEntity
        {
            var queryAble = DbContext.Set<TrdEntity>().AsQueryable();
            if (writeDb)
                queryAble = queryAble.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            if (noTracking)
                queryAble = queryAble.AsNoTracking();
            return queryAble;
        }

        public async Task<TEntity> FindAsync(long keyValue, Expression<Func<TEntity, dynamic>> navigationPropertyPath = null, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
        {
            var query = this.GetDbSet(writeDb, noTracking).Where(t => t.Id == keyValue);
            if (navigationPropertyPath != null)
                return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(EntityFrameworkQueryableExtensions.Include(query, navigationPropertyPath), cancellationToken);
            else
                return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, cancellationToken);
        }

        public async Task<TEntity> FetchAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, dynamic>> navigationPropertyPath = null, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
        {
            TEntity result;

            var query = this.GetDbSet(writeDb, noTracking).Where(whereExpression);

            if (navigationPropertyPath != null)
                query = EntityFrameworkQueryableExtensions.Include(query, navigationPropertyPath);

            if (orderByExpression == null)
                result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query.OrderByDescending(x => x.Id), cancellationToken);
            else
                result = ascending
                          ? await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query.OrderBy(orderByExpression), cancellationToken)
                          : await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query.OrderByDescending(orderByExpression), cancellationToken)
                          ;

            return result;
        }

        public async Task<TResult> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
        {
            TResult result;

            var query = this.GetDbSet(writeDb, noTracking).Where(whereExpression);

            if (orderByExpression == null)
                result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query.OrderByDescending(x => x.Id).Select(selector), cancellationToken);
            else
                result = ascending
                          ? await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query.OrderBy(orderByExpression).Select(selector), cancellationToken)
                          : await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query.OrderByDescending(orderByExpression).Select(selector), cancellationToken)
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

        public async Task<int> DeleteAsync(long keyValue, CancellationToken cancellationToken = default)
        {
            //获取实体是否被跟踪
            var entity = DbContext.Set<TEntity>().Local.FirstOrDefault(x => x.Id == keyValue);

            //if (entity == null)
            //    entity = new TEntity() { Id = keyValue };

            //DbContext.Remove(entity);
            //return await DbContext.SaveChangesAsync();

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

        public async Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
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

        public async Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default)
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

        public async Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default)
        {
            var mapping = DbContext.Model.FindEntityType(typeof(TEntity));
            var property = mapping.GetProperties().Where(p => p.Name == "RowVersion").FirstOrDefault();

            if (property != null)
            {
                throw new Exception("该实体有RowVersion列，不能使用批量更新");
            }
            return await DbContext.Set<TEntity>().Where(whereExpression).UpdateAsync(updatingExpression, cancellationToken);
        }
    }
}