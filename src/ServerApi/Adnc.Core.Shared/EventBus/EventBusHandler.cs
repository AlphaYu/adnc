using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Adnc.Core.Shared.EventBus
{
    public abstract class EventBusHandler : IEventHandler
    {
        protected Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions)
        {
            return expressions;
        }
    }
}
