using System.Text;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Adnc.Infra.Common.Extensions
{
    public static class StringBuilderExtension
    {
        /// <summary>A StringBuilder extension method that substrings.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>A string.</returns>
        public static string Substring([NotNull] this StringBuilder @this, int startIndex)
        {
            return @this.ToString(startIndex, @this.Length - startIndex);
        }

        /// <summary>A StringBuilder extension method that substrings.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>A string.</returns>
        public static string Substring([NotNull] this StringBuilder @this, int startIndex, int length)
        {
            return @this.ToString(startIndex, length);
        }

        /// <summary>A StringBuilder extension method that appends a join.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="values">The values.</param>
        public static StringBuilder AppendJoin<T>([NotNull] this StringBuilder @this, string separator, IEnumerable<T> values)
        {
            @this.Append(string.Join(separator, values));

            return @this;
        }

        /// <summary>A StringBuilder extension method that appends a line join.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="values">The values.</param>
        public static StringBuilder AppendLineJoin<T>([NotNull] this StringBuilder @this, string separator, IEnumerable<T> values)
        {
            @this.AppendLine(string.Join(separator, values));

            return @this;
        }
    }
}
