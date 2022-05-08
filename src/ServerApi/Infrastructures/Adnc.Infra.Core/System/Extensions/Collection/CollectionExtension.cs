namespace System.Collections.Generic;

/// <summary>
/// CollectionExtension
/// </summary>
public static class CollectionExtension
{
    /// <summary>
    ///     An ICollection&lt;T&gt; extension method that adds only if the value satisfies the predicate.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="predicate">The predicate.</param>
    /// <param name="value">The value.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static void AddIf<T>([NotNull] this ICollection<T> @this, Func<T, bool> predicate, T value)
    {
        if (@this.IsReadOnly)
            throw new InvalidOperationException($"{ nameof(@this) } is readonly");

        if (predicate(value))
            @this.Add(value);
    }

    /// <summary>
    ///     An ICollection&lt;T&gt; extension method that add value if the ICollection doesn't contains it already.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="value">The value.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static void AddIfNotContains<T>([NotNull] this ICollection<T> @this, T value)
    {
        if (@this.IsReadOnly)
            throw new InvalidOperationException($"{ nameof(@this) } is readonly");

        if (!@this.Contains(value))
            @this.Add(value);
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
            throw new InvalidOperationException($"{ nameof(@this) } is readonly");

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
        if (@this.IsReadOnly)
            throw new InvalidOperationException($"{ nameof(@this) } is readonly");

        foreach (var value in values.Where(predicate))
        {
            @this.Add(value);
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
        if (@this.IsReadOnly)
            throw new InvalidOperationException($"{ nameof(@this) } is readonly");

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
    public static bool IsNullOrEmpty<T>(this ICollection<T> @this) => @this == null || !@this.Any();

    /// <summary>
    ///     An ICollection&lt;T&gt; extension method that queries if the collection is not (null or is empty).
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if the collection is not (null or empty), false if not.</returns>
    public static bool IsNotNullOrEmpty<T>(this ICollection<T> @this) => @this != null && @this.Any();

    /// <summary>
    ///     An ICollection&lt;T&gt; extension method that removes the range.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="values">A variable-length parameters list containing values.</param>
    public static void RemoveRange<T>([NotNull] this ICollection<T> @this, params T[] values)
    {
        if (@this.IsReadOnly)
            throw new InvalidOperationException($"{ nameof(@this) } is readonly");

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
        if (@this.IsReadOnly)
            throw new InvalidOperationException($"{ nameof(@this) } is readonly");

        foreach (var value in values.Where(predicate))
        {
            @this.Remove(value);
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
        if (@this.IsReadOnly)
            throw new InvalidOperationException($"{ nameof(@this) } is readonly");

        var list = @this.Where(predicate).ToList();
        foreach (var item in list)
        {
            @this.Remove(item);
        }
    }
}