namespace System.Collections.Generic;

public static class IEnumerableExtensions
{
    /// <summary>
    /// 遍历IEnumerable
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var obj in source)
        {
            action(obj);
        }
    }

    /// <summary>
    /// 遍历IEnumerable
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static async Task ForEachAsync<T>(this IEnumerable<T> source, Action<T> action)
    {
        await Task.Run(() => Parallel.ForEach(source, action));
    }

    /// <summary>
    /// 按字段去重
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
    /// 在元素之后添加元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="condition">条件</param>
    /// <param name="value">值</param>
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
    /// 在元素之后添加元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="index">索引位置</param>
    /// <param name="value">值</param>
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
    /// 转HashSet
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

    public static string ToString<T>(this IEnumerable<T> source, string separator) => (source != null && source.Any()) ? string.Join(separator, source) : string.Empty;

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();

    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source) => source != null && source.Any();
}