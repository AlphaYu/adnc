using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Adnc.Core.Shared.Events
{
    public abstract class EventHandler : IEventHandler
    {
        protected Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions)
        {
            return expressions;
        }
    }
}
