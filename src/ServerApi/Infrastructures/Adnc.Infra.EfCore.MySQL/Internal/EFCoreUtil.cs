using System;
using System.Linq;

namespace Adnc.Infra.EfCore.Internal
{
    public static class EFCoreUtil
    {
        public static object[] GetEntityKeyValues<TEntity>(Func<TEntity, object>[] keyValueGetter, TEntity entity)
            => keyValueGetter.Select(x => x.Invoke(entity)).ToArray();
    }
}
