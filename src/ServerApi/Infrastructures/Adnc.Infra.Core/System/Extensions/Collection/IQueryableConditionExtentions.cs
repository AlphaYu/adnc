using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// Entity Framework LINQ related extension methods.
    /// </summary>
    public static class IQueryableConditionExtentions
    {
        #region WhereIf

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> @this, bool condition, [NotNull] Expression<Func<T, bool>> predicate)
            => condition ? @this.Where(predicate) : @this;

        #endregion WhereIf

        #region OrderByIf

        public static IOrderedQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource> @this, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => condition ? @this.OrderBy(keySelector, comparer) : @this.OrderBy(e => true);

        #endregion OrderByIf

        #region OrderByDescendingIf

        public static IOrderedQueryable<TSource> OrderByDescendingIf<TSource, TKey>(this IQueryable<TSource> @this, bool condition, Expression<Func<TSource, TKey>> keySelector)
            => condition ? @this.OrderByDescending(keySelector) : @this.OrderByDescending(e => true);

        public static IOrderedQueryable<TSource> OrderByDescendingIf<TSource, TKey>(this IQueryable<TSource> @this, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => condition ? @this.OrderByDescending(keySelector, comparer) : @this.OrderByDescending(e => true);

        #endregion OrderByDescendingIf

        #region ThenByIf

        public static IOrderedQueryable<TSource> ThenByIf<TSource, TKey>(this IOrderedQueryable<TSource> @this, bool condition, Expression<Func<TSource, TKey>> keySelector)
            => condition ? @this.ThenBy(keySelector) : @this;

        public static IOrderedQueryable<TSource> ThenByIf<TSource, TKey>(this IOrderedQueryable<TSource> @this, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => condition ? @this.ThenBy(keySelector, comparer) : @this;

        #endregion ThenByIf

        #region ThenByDescendingIf

        public static IOrderedQueryable<TSource> ThenByDescendingIf<TSource, TKey>(this IOrderedQueryable<TSource> @this, bool condition, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => condition ? @this.ThenByDescending(keySelector, comparer) : @this;

        public static IOrderedQueryable<TSource> ThenByDescendingIf<TSource, TKey>(this IOrderedQueryable<TSource> @this, bool condition, Expression<Func<TSource, TKey>> keySelector)
            => condition ? @this.ThenByDescending(keySelector) : @this;

        #endregion ThenByDescendingIf

        #region IsNullOrEmpty

        public static bool IsNullOrEmpty<T>(this IQueryable<T> @this)
            => (@this != null && @this.Any());

        #endregion IsNullOrEmpty
    }
}