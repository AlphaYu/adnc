namespace Adnc.Infra.Repository.EfCore.Repositories
{
    /// <summary>
    /// Ef默认的、全功能的仓储实现
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EfRepository<TEntity> : AbstractEfBaseRepository<DbContext, TEntity>, IEfRepository<TEntity>
      where TEntity : EfEntity, new()
    {
        private readonly IAdoQuerierRepository? _adoQuerier;

        public EfRepository(DbContext dbContext, IAdoQuerierRepository? adoQuerier = null)
            : base(dbContext)
            => _adoQuerier = adoQuerier;

        public IAdoQuerierRepository? AdoQuerier
        {
            get
            {
                if (_adoQuerier is null)
                    return null;
                if (!_adoQuerier.HasDbConnection())
                    _adoQuerier.ChangeOrSetDbConnection(DbContext.Database.GetDbConnection());
                return _adoQuerier;
            }
        }

        public async Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql, CancellationToken cancellationToken = default) =>
           await DbContext.Database.ExecuteSqlInterpolatedAsync(sql, cancellationToken);

        public async Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default) =>
           await DbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);

        public IDbTransaction? CurrentDbTransaction => DbContext.Database.CurrentTransaction?.GetDbTransaction();

        public virtual IQueryable<TEntity> GetAll(bool writeDb = false, bool noTracking = true) => this.GetDbSet(writeDb, noTracking);

        public virtual IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false, bool noTracking = true)
               where TrdEntity : EfEntity
        {
            var queryAble = DbContext.Set<TrdEntity>().AsQueryable();
            if (writeDb)
                queryAble = queryAble.TagWith(RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
            if (noTracking)
                queryAble = queryAble.AsNoTracking();
            return queryAble;
        }

        public virtual async Task<TEntity?> FindAsync(long keyValue, Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
        {
            var query = GetDbSet(writeDb, noTracking).Where(t => t.Id == keyValue);
            if (navigationPropertyPath is not null)
                return await query.Include(navigationPropertyPath).FirstOrDefaultAsync(cancellationToken);
            else
                return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, Expression<Func<TEntity, object>>? orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
        {
            TEntity? result;

            var query = GetDbSet(writeDb, noTracking).Where(whereExpression);

            if (navigationPropertyPath is not null)
                query = query.Include(navigationPropertyPath);

            if (orderByExpression is null)
                result = await query.OrderByDescending(x=>x.Id).FirstOrDefaultAsync(cancellationToken);
            else
                result = ascending
                          ? await query.OrderBy(orderByExpression).FirstOrDefaultAsync(cancellationToken)
                          : await query.OrderByDescending(orderByExpression).FirstOrDefaultAsync(cancellationToken)
                          ;

            return result;
        }

        public virtual async Task<TResult?> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>>? orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
        {
            TResult? result;

            var query = this.GetDbSet(writeDb, noTracking).Where(whereExpression);

            if (orderByExpression is null)
                result = await query.OrderByDescending(x => x.Id).Select(selector).FirstOrDefaultAsync(cancellationToken);
            else
                result = ascending
                          ? await query.OrderBy(orderByExpression).Select(selector).FirstOrDefaultAsync(cancellationToken)
                          : await query.OrderByDescending(orderByExpression).Select(selector).FirstOrDefaultAsync(cancellationToken)
                          ;

            return result;
        }

        public virtual async Task<int> DeleteAsync(long keyValue, CancellationToken cancellationToken = default)
        {
            int rows = 0;
            //查询当前上下文中，有没有同Id实体
            var entity = DbContext.Set<TEntity>().Local.FirstOrDefault(x => x.Id == keyValue);

            if (entity == null)
                entity = new TEntity { Id = keyValue };

            DbContext.Remove(entity);

            try
            {
                rows = await DbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                rows = 0;
            }
            return rows;

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

        public virtual async Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
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

        public virtual async Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default)
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

            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default)
        {
            var enityType = typeof(TEntity);
            var hasConcurrencyMember = typeof(IConcurrency).IsAssignableFrom(enityType);

            if (hasConcurrencyMember)
                throw new ArgumentException("该实体有RowVersion列，不能使用批量更新");

            return UpdateRangeInternalAsync(whereExpression, updatingExpression, cancellationToken);
        }

        public virtual async Task<int> UpdateRangeAsync(Dictionary<long, List<(string propertyName, dynamic propertyValue)>> propertyNameAndValues, CancellationToken cancellationToken = default)
        {
            var existsEntities = DbContext.Set<TEntity>().Local.Where(x => propertyNameAndValues.ContainsKey(x.Id));

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

            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        protected virtual async Task<int> UpdateRangeInternalAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<TEntity>().Where(whereExpression).UpdateAsync(updatingExpression, cancellationToken);
        }
    }
}