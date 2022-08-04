namespace Adnc.Infra.Repository.EfCore.Repositories;
/// <summary>
/// Ef默认的、全功能的仓储实现
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public sealed class EfNotKeyRepository<TEntity> : AbstractEfNotKeyBaseRepository<DbContext, TEntity>, IEfNotKeyRepository<TEntity>
  where TEntity : EfEntityNotKey, new()
{
    private readonly IAdoQuerierRepository? _adoQuerier;

    public EfNotKeyRepository(DbContext dbContext, IAdoQuerierRepository? adoQuerier = null)
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

    public IQueryable<TEntity> GetAll(bool writeDb = false, bool noTracking = true)
       => this.GetDbSet(writeDb, noTracking);

    public IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false, bool noTracking = true)
           where TrdEntity : EfEntityNotKey
    {
        var queryAble = DbContext.Set<TrdEntity>().AsQueryable();
        if (writeDb)
            queryAble = queryAble.TagWith(RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        if (noTracking)
            queryAble = queryAble.AsNoTracking();
        return queryAble;
    }

    public async Task<TResult> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
    {
        TResult? result;

        var query = this.GetDbSet(writeDb, noTracking).Where(whereExpression);

        if (orderByExpression is null)
            result = await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
        else
            result = ascending
                      ? await query.OrderBy(orderByExpression).Select(selector).FirstOrDefaultAsync(cancellationToken)
                      : await query.OrderByDescending(orderByExpression).Select(selector).FirstOrDefaultAsync(cancellationToken)
                      ;

        return result;
    }

    public async Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, bool isForceDel = false, CancellationToken cancellationToken = default)
    {
        if (isForceDel)
            return await DbContext.Set<TEntity>().Where(whereExpression).DeleteAsync(cancellationToken);
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
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default)
    {
        var enityType = typeof(TEntity);
        var hasConcurrencyMember = typeof(IConcurrency).IsAssignableFrom(enityType);

        if (hasConcurrencyMember)
            throw new ArgumentException("该实体有RowVersion列，不能使用批量更新");

        return UpdateRangeInternalAsync(whereExpression, updatingExpression, cancellationToken);
    }

    private async Task<int> UpdateRangeInternalAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default)
        => await DbContext.Set<TEntity>().Where(whereExpression).UpdateAsync(updatingExpression, cancellationToken);
}
