using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace Adnc.Infra.EfCore.Repositories
{
    /// <summary>
    /// Ef默认的、全功能的仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class EfRepository<TEntity> : AbstractEfBaseRepository<AdncDbContext, TEntity>, IEfRepository<TEntity>
      where TEntity : EfEntity, new()
    {
        public EfRepository(AdncDbContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<TEntity> GetAll(bool writeDb = false, bool noTracking = true)
            => this.GetDbSet(writeDb, noTracking);

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

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, dynamic>> navigationPropertyPath = null, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
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
        }

        public async Task<int> DeleteAsync(long keyValue, CancellationToken cancellationToken = default)
        {
            //查询当前上下文中，有没有同Id实体
            var entity = DbContext.Set<TEntity>().Local.FirstOrDefault(x => x.Id == keyValue);

            if (entity == null)
                entity = new TEntity { Id = keyValue };

            DbContext.Remove(entity);
            return await DbContext.SaveChangesAsync();

            #region old code

#pragma warning disable S125 // Sections of code should not be commented out
            /*
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
                        var isSoftDelete = properties.Any(p => p.Name == "IsDeleted");

                        var sql = isSoftDelete
                                  ? $"update {tableName} set IsDeleted=true "
                                  : $"delete from {tableName} "
                                  ;
                        var where = $" where {keyName}={keyValue};";

                        return await DbContext.Database.ExecuteSqlRawAsync(string.Concat(sql, where), cancellationToken);
                        */
#pragma warning restore S125 // Sections of code should not be commented outs

            #endregion old code
        }

        public async Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
        {
            var enityType = typeof(TEntity);
            var hasSoftDeleteMember = typeof(ISoftDelete).IsAssignableFrom(enityType);
            if (hasSoftDeleteMember)
            {
                var newExpression = Expression.New(enityType);
                var paramExpression = Expression.Parameter(enityType, "e");
                var binding = Expression.Bind(enityType.GetMember("IsDeleted")[0], Expression.Constant(true));
                var memberInitExpression = Expression.MemberInit(newExpression, new List<MemberBinding>() { binding });
                var lambdaExpression = Expression.Lambda<Func<TEntity, TEntity>>(memberInitExpression, paramExpression);
                return await DbContext.Set<TEntity>().Where(whereExpression).UpdateAsync(lambdaExpression, cancellationToken);
            }
            return await DbContext.Set<TEntity>().Where(whereExpression).DeleteAsync(cancellationToken);
        }

        public async Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default)
        {
            if (updatingExpressions.IsNullOrEmpty())
                await UpdateAsync(entity, cancellationToken);

            var entry = DbContext.Entry(entity);

            if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
                throw new ArgumentException($"{nameof(entity)},实体状态为{nameof(entry.State)}");

            if (entry.State == EntityState.Unchanged)
                return await Task.FromResult(0);

            if (entry.State == EntityState.Modified)
            {
                var propNames = updatingExpressions.Select(x => x.GetMemberName()).ToArray();
                entry.Properties.ForEach(propEntry =>
                {
                    if (!propNames.Contains(propEntry.Metadata.Name))
                        propEntry.IsModified = false;
                });
            }

            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Unchanged;
                updatingExpressions.ForEach(expression =>
                {
                    entry.Property(expression).IsModified = true;
                });
            }

            #region removed code

#pragma warning disable S125 // Sections of code should not be commented out
            /*
                            var originalEntity = DbContext.Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
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
                                entry.CurrentValues.SetValues(entity);
                                var propNames = updatingExpressions.Select(x => x.GetMemberName()).ToArray();
                                entry.Properties.ForEach(propEntry =>
                                {
                                    if (!propNames.Contains(propEntry.Metadata.Name))
                                        propEntry.IsModified = false;
                                });
                            }
                            */
#pragma warning restore S125 // Sections of code should not be commented out

            #endregion removed code

            return await DbContext.SaveChangesAsync();
        }

        public Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default)
        {
            var enityType = typeof(TEntity);
            var hasConcurrencyMember = typeof(IConcurrency).IsAssignableFrom(enityType);

            if (hasConcurrencyMember)
                throw new ArgumentException("该实体有RowVersion列，不能使用批量更新");

            return UpdateRangeInternalAsync(whereExpression, updatingExpression, cancellationToken);
        }

        public async Task<int> UpdateRangeAsync(Dictionary<long, List<(string propertyName, dynamic propertyValue)>> propertyNameAndValues, CancellationToken cancellationToken = default)
        {
            var existsEntities = DbContext.Set<TEntity>().Local.Where(x => propertyNameAndValues.Keys.Contains(x.Id));

            foreach (var item in propertyNameAndValues)
            {
                var enity = existsEntities?.FirstOrDefault(x => x.Id == item.Key) ?? new TEntity { Id = item.Key };
                var entry = DbContext.Entry(enity);
                if (entry.State == EntityState.Detached)
                    entry.State = EntityState.Unchanged;

                if (entry.State == EntityState.Unchanged)
                {
                    var info = propertyNameAndValues.FirstOrDefault(x => x.Key == item.Key).Value;
                    info.ForEach(x =>
                    {
                        entry.Property(x.propertyName).CurrentValue = x.propertyValue;
                        entry.Property(x.propertyName).IsModified = true;
                    });
                }
            }

            return await DbContext.SaveChangesAsync();
        }

        private async Task<int> UpdateRangeInternalAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default)
            => await DbContext.Set<TEntity>().Where(whereExpression).UpdateAsync(updatingExpression, cancellationToken);
    }
}