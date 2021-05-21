using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Infra.EfCore.Repositories
{
    /// <summary>
    /// Ef简单的、基础的，初级的仓储接口
    /// 适合DDD开发模式,实体必须继承AggregateRoot
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class EfBasicRepository<TEntity> : AbstractEfBaseRepository<AdncDbContext, TEntity>, IEfBasicRepository<TEntity>
            where TEntity : AggregateRoot
    {
        public EfBasicRepository(AdncDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            this.DbContext.UpdateRange(entities);
            return await this.DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            this.DbContext.Remove(entity);
            return await this.DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            this.DbContext.RemoveRange(entities);
            return await this.DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity> GetAsync(long keyValue, Expression<Func<TEntity, dynamic>> navigationPropertyPath = null, bool writeDb = false, CancellationToken cancellationToken = default)
        {
            var query = this.GetDbSet(writeDb, false).Where(t => t.Id == keyValue);
            if (navigationPropertyPath != null)
                return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(EntityFrameworkQueryableExtensions.Include(query, navigationPropertyPath), cancellationToken);
            else
                return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, cancellationToken);
        }
    }
}