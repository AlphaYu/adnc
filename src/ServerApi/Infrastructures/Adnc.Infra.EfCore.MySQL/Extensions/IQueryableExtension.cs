using JetBrains.Annotations;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Adnc.Infra.EfCore.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> @this, bool condition, Expression<Func<T, bool>> predicate)
            => condition? @this.Where(predicate): @this;
    }
}