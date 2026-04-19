using Adnc.Infra.Repository.EfCore.Extensions;
using Microsoft.EntityFrameworkCore.Query;

namespace Adnc.Infra.Repository.EfCore;

/// <summary>
/// Default full-featured EF repository implementation
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EfRepository<TEntity>(DbContext dbContext, Operater operater, IAdoQuerierRepository? adoQuerier = null) : AbstractEfBaseRepository<DbContext, TEntity>(dbContext), IEfRepository<TEntity>
  where TEntity : EfEntity, new()
{
    private readonly IAdoQuerierRepository? _adoQuerier = adoQuerier;

    public IAdoQuerierRepository AdoQuerier
    {
        get
        {
            if (_adoQuerier is null)
            {
                throw new ArgumentNullException(nameof(_adoQuerier));
            }

            if (!_adoQuerier.HasDbConnection())
            {
                _adoQuerier.ChangeOrSetDbConnection(DbContext.Database.GetDbConnection());
            }

            return _adoQuerier;
        }
    }

    public async Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql, CancellationToken cancellationToken = default)
       => await DbContext.Database.ExecuteSqlInterpolatedAsync(sql, cancellationToken);

    public async Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default)
       => await DbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);

    public IDbTransaction? CurrentDbTransaction => DbContext.Database.CurrentTransaction?.GetDbTransaction();

    public virtual IQueryable<TEntity> GetAll(bool writeDb = false, bool noTracking = true)
        => GetDbSet(writeDb, noTracking);

    public virtual IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false, bool noTracking = true) where TrdEntity : EfEntity
        => GetDbSet<TrdEntity>(writeDb, noTracking);

    public virtual async Task<TEntity?> FetchAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, Expression<Func<TEntity, object>>? orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
    {
        TEntity? result;

        var query = GetDbSet(writeDb, noTracking).Where(whereExpression);

        if (navigationPropertyPath is not null)
        {
            query = query.Include(navigationPropertyPath);
        }

        if (orderByExpression is null)
        {
            result = await query.OrderByDescending(x => x.Id).FirstOrDefaultAsync(cancellationToken);
        }
        else
        {
            result = ascending
                      ? await query.OrderBy(orderByExpression).FirstOrDefaultAsync(cancellationToken)
                      : await query.OrderByDescending(orderByExpression).FirstOrDefaultAsync(cancellationToken)
                      ;
        }

        return result;
    }

    public virtual async Task<TResult?> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>>? orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default)
    {
        TResult? result;

        var query = GetDbSet(writeDb, noTracking).Where(whereExpression);

        if (orderByExpression is null)
        {
            result = await query.OrderByDescending(x => x.Id).Select(selector).FirstOrDefaultAsync(cancellationToken);
        }
        else
        {
            result = ascending
                      ? await query.OrderBy(orderByExpression).Select(selector).FirstOrDefaultAsync(cancellationToken)
                      : await query.OrderByDescending(orderByExpression).Select(selector).FirstOrDefaultAsync(cancellationToken)
                      ;
        }

        return result;
    }

    public virtual async Task<int> DeleteAsync(long keyValue, CancellationToken cancellationToken = default)
    {
        var rows = 0;
        // Check whether the same entity already exists in the current context by Id
        var entity = DbContext.Set<TEntity>().Local.FirstOrDefault(x => x.Id == keyValue);

        entity ??= new TEntity { Id = keyValue };

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
    }

    public virtual async Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
    {
        //var enityType = typeof(TEntity);
        //var hasSoftDeleteMember = typeof(ISoftDelete).IsAssignableFrom(enityType);
        //if (hasSoftDeleteMember)
        //{
        //    var newExpression = Expression.New(enityType);
        //    var paramExpression = Expression.Parameter(enityType, "e");
        //    var binding = Expression.Bind(enityType.GetMember("IsDeleted")[0], Expression.Constant(true));
        //    var memberInitExpression = Expression.MemberInit(newExpression, new List<MemberBinding>() { binding });
        //    var lambdaExpression = Expression.Lambda<Func<TEntity, TEntity>>(memberInitExpression, paramExpression);
        //    return await DbContext.Set<TEntity>().Where(whereExpression).UpdateAsync(lambdaExpression, cancellationToken);
        //}
        //return await DbContext.Set<TEntity>().Where(whereExpression).DeleteAsync(cancellationToken);

        //efcore>=7.0.0, don't support soft delete
        var queryAble = DbContext.Set<TEntity>().Where(whereExpression);
        var enityType = typeof(TEntity);
        var hasSoftDeleteMember = typeof(ISoftDelete).IsAssignableFrom(enityType);
        if (!hasSoftDeleteMember)
        {
            return await queryAble.ExecuteDeleteAsync(cancellationToken);
        }
        else
        {
            var hasfullAuditInfoMember = typeof(IFullAuditInfo).IsAssignableFrom(enityType);
            if (!hasfullAuditInfoMember)
            {
                return await queryAble.ExecuteUpdateAsync(setters => setters.SetProperty(setter => ((ISoftDelete)setter).IsDeleted, true), cancellationToken);
            }
            else
            {
                return await queryAble.ExecuteUpdateAsync(setters => setters
                .SetProperty(setter => ((ISoftDelete)setter).IsDeleted, true)
                .SetProperty(setter => ((IFullAuditInfo)setter).ModifyBy, operater.Id)
                .SetProperty(setter => ((IFullAuditInfo)setter).ModifyTime, DateTime.Now), cancellationToken);
            }
        }
    }

    public virtual async Task<int> UpdateRangeAsync(Dictionary<long, List<(string propertyName, dynamic propertyValue)>> propertyNameAndValues, CancellationToken cancellationToken = default)
    {
        var existsEntities = DbContext.Set<TEntity>().Local.Where(x => propertyNameAndValues.ContainsKey(x.Id));

        foreach (var item in propertyNameAndValues)
        {
            var enity = existsEntities?.FirstOrDefault(x => x.Id == item.Key) ?? new TEntity { Id = item.Key };
            var entry = DbContext.Entry(enity);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Unchanged;
            }

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

    public virtual async Task<int> ExecuteUpdateAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default)
    {
        //efcore>=7.0.0
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> newSetPropertyCalls;
        var enityType = typeof(TEntity);
        var hasfullAuditInfoMember = typeof(IFullAuditInfo).IsAssignableFrom(enityType);
        if (!hasfullAuditInfoMember)
        {
            newSetPropertyCalls = setPropertyCalls;
        }
        else
        {
            newSetPropertyCalls = setPropertyCalls
                .Append(setter => setter.SetProperty(setter => ((IFullAuditInfo)setter).ModifyBy, operater.Id))
                .Append(setter => setter.SetProperty(setter => ((IFullAuditInfo)setter).ModifyTime, DateTime.Now));
        }

        return await DbContext.Set<TEntity>().Where(whereExpression).ExecuteUpdateAsync(newSetPropertyCalls, cancellationToken);
    }
}
