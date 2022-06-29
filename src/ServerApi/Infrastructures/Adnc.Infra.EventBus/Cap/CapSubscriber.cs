using DotNetCore.CAP;

namespace Adnc.Infra.EventBus.Cap
{
    public abstract class CapSubscriber : IEventSubscriber, ICapSubscribe
    {
        protected Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions)
            => expressions;
    }
}