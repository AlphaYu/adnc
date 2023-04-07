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
            throw new InvalidOperationException($"{nameof(@this)} is readonly");

        if (predicate(value))
            @this.Add(value);
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
}