namespace System.Collections.Generic;

/// <summary>
/// Defines a static class that adds extension methods to the IEnumerable type.
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// Traverse an IEnumerable and perform an action on each element.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="action">The callback method.</param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var obj in source)
        {
            action(obj);
        }
    }

    /// <summary>
    /// Traverse an IEnumerable asynchronously and perform an action on each element.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="action">The callback method.</param>
    /// <typeparam name="T"></typeparam>
    public static async Task ForEachAsync<T>(this IEnumerable<T> source, Action<T> action)
    {
        await Task.Run(() => Parallel.ForEach(source, action));
    }

    /// <summary>
    /// Remove duplicates based on a specific key.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        var hash = new HashSet<TKey>();
        return source.Where(p => hash.Add(keySelector(p)));
    }

    /// <summary>
    /// Insert an element after a specified element in a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="condition">The condition to determine the element to insert after.</param>
    /// <param name="value">The value to insert.</param>
    public static void InsertAfter<T>(this IList<T> list, Func<T, bool> condition, T value)
    {
        foreach (var item in list.Select((item, index) => new { item, index }).Where(p => condition(p.item)).OrderByDescending(p => p.index))
        {
            if (item.index + 1 == list.Count)
            {
                list.Add(value);
            }
            else
            {
                list.Insert(item.index + 1, value);
            }
        }
    }

    /// <summary>
    /// Insert an element after a specified index in a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="index">The index to insert after.</param>
    /// <param name="value">The value to insert.</param>
    public static void InsertAfter<T>(this IList<T> source, int index, T value)
    {
        foreach (var item in source.Select((v, i) => new { Value = v, Index = i }).Where(p => p.Index == index).OrderByDescending(p => p.Index))
        {
            if (item.Index + 1 == source.Count)
                source.Add(value);
            else
                source.Insert(item.Index + 1, value);
        }
    }

    /// <summary>
    /// Convert an IEnumerable to a HashSet.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static HashSet<TResult> ToHashSet<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
    {
        var set = new HashSet<TResult>();
        set.UnionWith(source.Select(selector));
        return set;
    }

    /// <summary>
    /// Converts an IEnumerable&lt;T&gt; object to a string, where T is the type parameter.
    /// </summary>
    /// <param name="source">The IEnumerable&lt;T&gt; object to convert.</param>
    /// <param name="separator">The separator between each element in the resulting string.</param>
    /// <returns>
    /// The string representation of the elements in source, separated by separator, or an empty string if source is null or empty.
    /// </returns>
    public static string ToString<T>(this IEnumerable<T> source, string separator) =>
        (source != null && source.Any()) ? string.Join(separator, source) : string.Empty;

    /// <summary>
    /// Checks whether an IEnumerable&lt;T&gt; object is null or empty, where T is the type parameter.
    /// </summary>
    /// <param name="source">The IEnumerable&lt;T&gt; object to check.</param>
    /// <returns>
    /// true if source is null or empty; otherwise, false.
    /// </returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();

    /// <summary>
    /// Checks whether an IEnumerable&lt;T&gt; object is not null and not empty, where T is the type parameter.
    /// </summary>
    /// <param name="source">The IEnumerable&lt;T&gt; object to check.</param>
    /// <returns>
    /// true if source is not null and has at least one element; otherwise, false.
    /// </returns>
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source) => source != null && source.Any();
}