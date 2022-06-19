namespace System
{
    public static class GuidExtension
    {
        /// <summary>A GUID extension method that query if '@this' is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return guid is null || !guid.HasValue || guid == Guid.Empty;
        }

        /// <summary>A GUID extension method that query if '@this' is not null or empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsNotNullOrEmpty(this Guid? guid)
        {
            return guid.HasValue && guid.Value != Guid.Empty;
        }

        /// <summary>A GUID extension method that query if '@this' is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>A GUID extension method that queries if a not is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if a not is empty, false if not.</returns>
        public static bool IsNotEmpty(this Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}