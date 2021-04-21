using System;
using System.Linq.Expressions;

namespace Adnc.Core.Shared
{
    public interface ICoreService { }

    public abstract class AbstractCoreService : ICoreService
    {
        protected Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions)
        {
            return expressions;
        }
    }
}


