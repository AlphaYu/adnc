namespace System.Collections.Generic
{
    public static class CollectionExtension
    {
        /// <summary>
        /// Adds the specified value to the collection if the given predicate is true.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection to add the value to.</param>
        /// <param name="predicate">The predicate to evaluate.</param>
        /// <param name="value">The value to add to the collection.</param>
        /// <exception cref="ArgumentNullException">Thrown when the collection is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the collection is read-only.</exception>
        public static void AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T value)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection.IsReadOnly)
            {
                throw new InvalidOperationException($"{nameof(collection)} is readonly");
            }

            if (predicate(value))
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// Determines whether the collection contains all the specified values.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <param name="values">The values to check for in the collection.</param>
        /// <returns>True if the collection contains all the specified values; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the collection or the values parameter is null.</exception>
        public static bool ContainsAll<T>(this ICollection<T> collection, params T[] values)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (var value in values)
            {
                if (!collection.Contains(value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the collection contains any of the specified values.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <param name="values">The values to check for in the collection.</param>
        /// <returns>True if the collection contains any of the specified values; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the collection or the values parameter is null.</exception>
        public static bool ContainsAny<T>(this ICollection<T> collection, params T[] values)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (var value in values)
            {
                if (collection.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns>True if the collection is null or empty; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection == null || !collection.Any();

        /// <summary>
        /// Determines whether the collection is not null or not empty.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns>True if the collection is not null and not empty; otherwise, false.</returns>
        public static bool IsNotNullOrEmpty<T>(this ICollection<T> collection) => collection != null && collection.Any();
    }
}
