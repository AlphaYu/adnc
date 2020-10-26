using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Adnc.Infr.Common.Extensions
{
    /// <summary>
    /// CollectionExtension
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        ///     A NameValueCollection extension method that converts the @this to a dictionary.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an IDictionary&lt;string,object&gt;</returns>
        public static IDictionary<string, string> ToDictionary(this NameValueCollection @this)
        {
            var dict = new Dictionary<string, string>();

            if (@this != null)
            {
                foreach (var key in @this.AllKeys)
                {
                    dict.Add(key, @this[key]);
                }
            }

            return dict;
        }

        /// <summary>将名值集合转换成字符串，key1=value1&amp;key2=value2，k/v会编码</summary>
        /// <param name="source">数据源</param>
        /// <returns>字符串</returns>

        public static string ToQueryString(this NameValueCollection source)
        {
            if (source == null)
            {
                return null;
            }

            var sb = new StringBuilder(1024);

            foreach (var key in source.AllKeys)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }
                sb.Append("&");
                sb.Append(WebUtility.UrlEncode(key));
                sb.Append("=");
                var val = source.Get(key);
                if (val != null)
                {
                    sb.Append(WebUtility.UrlEncode(val));
                }
            }

            return sb.Length > 0 ? sb.ToString(1, sb.Length - 1) : "";
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that adds only if the value satisfies the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool AddIf<T>([NotNull] this ICollection<T> @this, Func<T, bool> predicate, T value)
        {
            if (@this.IsReadOnly) return false;
            if (predicate(value))
            {
                @this.Add(value);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that add value if the ICollection doesn't contains it already.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool AddIfNotContains<T>([NotNull] this ICollection<T> @this, T value)
        {
            if (@this.IsReadOnly) return false;
            if (!@this.Contains(value))
            {
                @this.Add(value);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that adds a range to 'values'.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void AddRange<T>([NotNull] this ICollection<T> @this, params T[] values)
        {
            if (@this.IsReadOnly)
            {
                return;
            }
            foreach (var value in values)
            {
                @this.Add(value);
            }
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that adds a collection of objects to the end of this collection only
        ///     for value who satisfies the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void AddRangeIf<T>([NotNull] this ICollection<T> @this, Func<T, bool> predicate, params T[] values)
        {
            if (@this.IsReadOnly) return;
            foreach (var value in values)
            {
                if (predicate(value))
                {
                    @this.Add(value);
                }
            }
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that adds a range of values that's not already in the ICollection.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void AddRangeIfNotContains<T>([NotNull] this ICollection<T> @this, params T[] values)
        {
            if (@this.IsReadOnly) return;
            foreach (var value in values)
            {
                if (!@this.Contains(value))
                {
                    @this.Add(value);
                }
            }
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that query if '@this' contains all values.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAll<T>([NotNull] this ICollection<T> @this, params T[] values)
        {
            foreach (var value in values)
            {
                if (!@this.Contains(value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that query if '@this' contains any value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool ContainsAny<T>([NotNull] this ICollection<T> @this, params T[] values)
        {
            foreach (var value in values)
            {
                if (@this.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that queries if the collection is null or is empty.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null or empty&lt; t&gt;, false if not.</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> @this)
        {
            return @this == null || @this.Count == 0;
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that queries if the collection is not (null or is empty).
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if the collection is not (null or empty), false if not.</returns>
        public static bool IsNotNullOrEmpty<T>(this ICollection<T> @this)
        {
            return @this != null && @this.Count != 0;
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that removes if.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <param name="predicate">The predicate.</param>
        public static void RemoveIf<T>([NotNull] this ICollection<T> @this, T value, Func<T, bool> predicate)
        {
            if (@this.IsReadOnly) return;
            if (predicate(value))
            {
                @this.Remove(value);
            }
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that removes if contains.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        public static void RemoveIfContains<T>([NotNull] this ICollection<T> @this, T value)
        {
            if (@this.IsReadOnly) return;
            if (@this.Contains(value))
            {
                @this.Remove(value);
            }
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that removes the range.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void RemoveRange<T>([NotNull] this ICollection<T> @this, params T[] values)
        {
            foreach (var value in values)
            {
                @this.Remove(value);
            }
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that removes range item that satisfy the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void RemoveRangeIf<T>([NotNull] this ICollection<T> @this, Func<T, bool> predicate, params T[] values)
        {
            if (@this.IsReadOnly) return;
            foreach (var value in values)
            {
                if (predicate(value))
                {
                    @this.Remove(value);
                }
            }
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that removes the range if contains.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void RemoveRangeIfContains<T>([NotNull] this ICollection<T> @this, params T[] values)
        {
            if (@this.IsReadOnly) return;
            foreach (var value in values)
            {
                if (@this.Contains(value))
                {
                    @this.Remove(value);
                }
            }
        }

        /// <summary>
        ///     An ICollection&lt;T&gt; extension method that removes value that satisfy the predicate.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        public static void RemoveWhere<T>([NotNull] this ICollection<T> @this, Func<T, bool> predicate)
        {
            if (@this.IsReadOnly) return;
            var list = @this.Where(predicate).ToList();
            foreach (var item in list)
            {
                @this.Remove(item);
            }
        }

        /// <summary>
        /// 获取随机排序后列表
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="list">list</param>
        /// <returns></returns>
        private static List<T> GetRandomList<T>([NotNull] this IList<T> list)
        {
            return Enumerable.Range(0, list.Count).OrderBy(_ => Helper.SecurityHelper.Random.Next(list.Count)).Select(i => list[i]).ToList();
        }
    }
}
