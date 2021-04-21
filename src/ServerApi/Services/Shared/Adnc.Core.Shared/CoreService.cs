using System;
using System.Linq.Expressions;

namespace Adnc.Core.Shared
{
    public abstract class CoreService : ICoreService
    {
        protected Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions)
        {
            return expressions;
        }
    }
}
