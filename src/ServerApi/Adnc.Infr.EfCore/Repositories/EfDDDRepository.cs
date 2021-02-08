using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.IRepositories;

namespace Adnc.Infr.EfCore.Repositories
{
    public class EfDDDRepository<TEntity> : EfRepository<TEntity>, IEfRepository<TEntity>
      where TEntity : EfEntity
    {
        public EfDDDRepository(AdncDbContext dbContext)
            : base(dbContext)
        {
        }

        [Obsolete("该方法已被弃用", true)]
        public override async Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> upDateExpression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        [Obsolete("该方法已被弃用", true)]
        public override async Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [Obsolete("该方法已被弃用", true)]
        public override IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb, bool noTracking)
        {
            throw new NotImplementedException();
        }

        [Obsolete("该方法已被弃用", true)]
        public override async Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[] propertyExpressions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
