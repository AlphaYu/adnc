using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Core.Shared.IRepositories
{
    /// <summary>
    /// Queryable.ToListAsync()
    /// 参考 Efcore EntityFrameworkQueryableExtensions.cs
    /// </summary>
    public static class EfCoreIQueryableExtentions
    {

        public static async Task<List<TSource>> ToListAsync<TSource>([NotNull] this IQueryable<TSource> source
            , CancellationToken cancellationToken = default)
        {
            var list = new List<TSource>();
            await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                list.Add(element);
            }

            return list;
        }

        /// <summary>
        ///     Returns an <see cref="IAsyncEnumerable{T}" /> which can be enumerated asynchronously.
        /// </summary>
        /// <remarks>
        ///     Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///     that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <typeparam name="TSource">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IQueryable{T}" /> to enumerate.
        /// </param>
        /// <returns> The query results. </returns>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="source" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" /> is not a <see cref="IAsyncEnumerable{T}" />.
        /// </exception>
        public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(
            [NotNull] this IQueryable<TSource> source)
        {
            if (ReferenceEquals(source, null))
                throw new ArgumentNullException(nameof(source));

            if (source is IAsyncEnumerable<TSource> asyncEnumerable)
            {
                return asyncEnumerable;
            }

            throw new InvalidOperationException($"The source IQueryable doesn't implement IAsyncEnumerable<{typeof(TSource)}>. Only sources that implement IAsyncEnumerable can be used for Entity Framework asynchronous operations.");
        }
    }
}
