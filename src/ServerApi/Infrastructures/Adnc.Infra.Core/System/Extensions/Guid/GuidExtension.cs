using System.Diagnostics.CodeAnalysis;

namespace System
{
    public static class GuidExtension
    {
        /// <summary>A GUID extension method that query if '@this' is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsNullOrEmpty([NotNull] this Guid? @this)
        {
            return !@this.HasValue || @this == Guid.Empty;
        }

        /// <summary>A GUID extension method that query if '@this' is not null or empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsNotNullOrEmpty([NotNull] this Guid? @this)
        {
            return @this.HasValue && @this.Value != Guid.Empty;
        }

        /// <summary>A GUID extension method that query if '@this' is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsEmpty(this Guid @this)
        {
            return @this == Guid.Empty;
        }

        /// <summary>A GUID extension method that queries if a not is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if a not is empty, false if not.</returns>
        public static bool IsNotEmpty(this Guid @this)
        {
            return @this != Guid.Empty;
        }
    }
}