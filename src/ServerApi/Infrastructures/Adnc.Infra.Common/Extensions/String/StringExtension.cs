using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Adnc.Infra.Common.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        ///     A T extension method that query if '@this' is null.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null, false if not.</returns>
        public static bool IsNull(this string @this)
        {
            return @this == null;
        }

        /// <summary>
        ///     A string extension method that query if '@this' is null or empty.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null or empty, false if not.</returns>
        public static bool IsNullOrEmpty(this string @this) => string.IsNullOrEmpty(@this);

        /// <summary>
        ///     A string extension method that query if '@this' is not null and not empty.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>false if null or empty, true if not.</returns>
        public static bool IsNotNullOrEmpty(this string @this)
            => !string.IsNullOrEmpty(@this);

        /// <summary>
        ///     A string extension method that query if '@this' is null or whiteSpace.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null or whiteSpace, false if not.</returns>
        public static bool IsNullOrWhiteSpace(this string @this) => string.IsNullOrWhiteSpace(@this);

        /// <summary>
        ///     A string extension method that query if '@this' is not null and not whiteSpace.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>false if null or whiteSpace, true if not.</returns>
        public static bool IsNotNullOrWhiteSpace(this string @this) => !string.IsNullOrWhiteSpace(@this);

        /// <summary>
        ///     Creates a new instance of  with the same value as a specified .
        /// </summary>
        /// <param name="str">The string to copy.</param>
        /// <returns>A new string with the same value as .</returns>
        public static string Copy([NotNull] this string str) => string.Copy(str);

        /// <summary>
        ///     Retrieves the system&#39;s reference to the specified .
        /// </summary>
        /// <param name="str">A string to search for in the intern pool.</param>
        /// <returns>
        ///     The system&#39;s reference to , if it is interned; otherwise, a new reference to a string with the value of .
        /// </returns>
        public static string Intern([NotNull] this string str) => string.Intern(str);

        /// <summary>
        ///     Retrieves a reference to a specified .
        /// </summary>
        /// <param name="str">The string to search for in the intern pool.</param>
        /// <returns>A reference to  if it is in the common language runtime intern pool; otherwise, null.</returns>
        public static string IsInterned([NotNull] this string str) => string.IsInterned(str);

        /// <summary>
        ///     Concatenates the elements of an object array, using the specified separator between each element.
        /// </summary>
        /// <param name="separator">
        ///     The string to use as a separator.  is included in the returned string only if  has more
        ///     than one element.
        /// </param>
        /// <param name="values">An array that contains the elements to concatenate.</param>
        /// <returns>
        ///     A string that consists of the elements of  delimited by the  string. If  is an empty array, the method
        ///     returns .
        /// </returns>
        public static string Join<T>([NotNull] this string separator, IEnumerable<T> values) => string.Join(separator, values);

        /// <summary>
        ///     Indicates whether the specified regular expression finds a match in the specified input string.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <returns>true if the regular expression finds a match; otherwise, false.</returns>
        public static bool IsMatch([NotNull] this string input, string pattern) => Regex.IsMatch(input, pattern);

        /// <summary>
        ///     Indicates whether the specified regular expression finds a match in the specified input string, using the
        ///     specified matching options.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="options">A bitwise combination of the enumeration values that provide options for matching.</param>
        /// <returns>true if the regular expression finds a match; otherwise, false.</returns>
        public static bool IsMatch([NotNull] this string input, string pattern, RegexOptions options) => Regex.IsMatch(input, pattern, options);

        /// <summary>An IEnumerable&lt;string&gt; extension method that concatenates the given this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        public static string Concatenate([NotNull] this IEnumerable<string> @this)
        {
            var sb = new StringBuilder();

            foreach (var s in @this)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }

        /// <summary>An IEnumerable&lt;T&gt; extension method that concatenates.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="func">The function.</param>
        /// <returns>A string.</returns>
        public static string Concatenate<T>([NotNull] this IEnumerable<T> source, Func<T, string> func)
        {
            var sb = new StringBuilder();
            foreach (var item in source)
            {
                sb.Append(func(item));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     A string extension method that query if this object contains the given value.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the value is in the string, false if not.</returns>
        public static bool Contains([NotNull] this string @this, string value) => @this.IndexOf(value, StringComparison.Ordinal) != -1;

        /// <summary>
        ///     A string extension method that query if this object contains the given value.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>true if the value is in the string, false if not.</returns>
        public static bool Contains([NotNull] this string @this, string value, StringComparison comparisonType) => @this.IndexOf(value, comparisonType) != -1;

        /// <summary>
        ///     A string extension method that extracts this object.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A string.</returns>
        public static string Extract([NotNull] this string @this, Func<char, bool> predicate) => new string(@this.ToCharArray().Where(predicate).ToArray());

        /// <summary>
        ///     A string extension method that removes the letter.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A string.</returns>
        public static string RemoveWhere([NotNull] this string @this, Func<char, bool> predicate) => new string(@this.ToCharArray().Where(x => !predicate(x)).ToArray());

        /// <summary>
        ///     Replaces the format item in a specified String with the text equivalent of the value of a corresponding
        ///     Object instance in a specified array.
        /// </summary>
        /// <param name="this">A String containing zero or more format items.</param>
        /// <param name="values">An Object array containing zero or more objects to format.</param>
        /// <returns>
        ///     A copy of format in which the format items have been replaced by the String equivalent of the corresponding
        ///     instances of Object in args.
        /// </returns>
        public static string FormatWith([NotNull] this string @this, params object[] values) => string.Format(@this, values);

        /// <summary>
        ///     A string extension method that query if '@this' satisfy the specified pattern.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="pattern">The pattern to use. Use '*' as wildcard string.</param>
        /// <returns>true if '@this' satisfy the specified pattern, false if not.</returns>
        public static bool IsLike([NotNull] this string @this, string pattern)
        {
            // Turn the pattern into regex pattern, and match the whole string with ^$
            var regexPattern = "^" + Regex.Escape(pattern) + "$";

            // Escape special character ?, #, *, [], and [!]
            regexPattern = regexPattern.Replace(@"\[!", "[^")
                .Replace(@"\[", "[")
                .Replace(@"\]", "]")
                .Replace(@"\?", ".")
                .Replace(@"\*", ".*")
                .Replace(@"\#", @"\d");

            return Regex.IsMatch(@this, regexPattern);
        }

        /// <summary>
        /// SafeSubstring
        /// </summary>
        /// <param name="this"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string SafeSubstring([NotNull] this string @this, int startIndex)
        {
            if (startIndex < 0 || startIndex > @this.Length)
            {
                return string.Empty;
            }
            return @this.Substring(startIndex);
        }

        /// <summary>
        /// SafeSubstring
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SafeSubstring([NotNull] this string str, int startIndex, int length)
        {
            if (startIndex < 0 || startIndex >= str.Length || length < 0)
            {
                return string.Empty;
            }
            return str.Substring(startIndex, Math.Min(str.Length - startIndex, length));
        }

        /// <summary>
        /// Sub, not only substring but support for negative numbers
        /// </summary>
        /// <param name="this">string to be handled</param>
        /// <param name="startIndex">startIndex to substract</param>
        /// <returns>substring</returns>
        public static string Sub([NotNull] this string @this, int startIndex)
        {
            if (startIndex >= 0)
            {
                return @this.SafeSubstring(startIndex);
            }
            if (Math.Abs(startIndex) > @this.Length)
            {
                return string.Empty;
            }
            return @this.Substring(@this.Length + startIndex);
        }

        /// <summary>
        ///     A string extension method that repeats the string a specified number of times.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="repeatCount">Number of repeats.</param>
        /// <returns>The repeated string.</returns>
        public static string Repeat([NotNull] this string @this, int repeatCount)
        {
            if (@this.Length == 1)
            {
                return new string(@this[0], repeatCount);
            }

            var sb = new StringBuilder(repeatCount * @this.Length);
            while (repeatCount-- > 0)
            {
                sb.Append(@this);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     A string extension method that reverses the given string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The string reversed.</returns>
        public static string Reverse([NotNull] this string @this)
        {
            if (@this.Length <= 1)
            {
                return @this;
            }

            var chars = @this.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        ///     Returns a String array containing the substrings in this string that are delimited by elements of a specified
        ///     String array. A parameter specifies whether to return empty array elements.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="separator">A string that delimit the substrings in this string.</param>
        /// <param name="option">
        ///     (Optional) Specify RemoveEmptyEntries to omit empty array elements from the array returned,
        ///     or None to include empty array elements in the array returned.
        /// </param>
        /// <returns>
        ///     An array whose elements contain the substrings in this string that are delimited by the separator.
        /// </returns>
        public static string[] Split([NotNull] this string @this, string separator, StringSplitOptions option = StringSplitOptions.None) => @this.Split(new[] { separator }, option);

        /// <summary>
        ///     A string extension method that converts the @this to a byte array.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a byte[].</returns>
        public static byte[] ToByteArray([NotNull] this string @this) => Encoding.UTF8.GetBytes(@this);

        /// <summary>
        ///     A string extension method that converts the @this to a byte array.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">encoding</param>
        /// <returns>@this as a byte[].</returns>
        public static byte[] ToByteArray([NotNull] this string @this, Encoding encoding) => encoding.GetBytes(@this);

        public static byte[] GetBytes([NotNull] this string str) => str.GetBytes(Encoding.UTF8);

        public static byte[] GetBytes([NotNull] this string str, Encoding encoding) => encoding.GetBytes(str);

        /// <summary>
        ///     A string extension method that converts the @this to an enum.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a T.</returns>
        public static T ToEnum<T>([NotNull] this string @this) => (T)Enum.Parse(typeof(T), @this);

        /// <summary>
        ///     A string extension method that converts the @this to a title case.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a string.</returns>
        public static string ToTitleCase([NotNull] this string @this) => new CultureInfo("en-US").TextInfo.ToTitleCase(@this);

        /// <summary>
        ///     A string extension method that converts the @this to a title case.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cultureInfo">Information describing the culture.</param>
        /// <returns>@this as a string.</returns>
        public static string ToTitleCase([NotNull] this string @this, CultureInfo cultureInfo) => cultureInfo.TextInfo.ToTitleCase(@this);

        /// <summary>
        ///     A string extension method that truncates.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>A string.</returns>
        public static string Truncate(this string @this, int maxLength) => @this.Truncate(maxLength, "...");

        /// <summary>
        ///     A string extension method that truncates.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>A string.</returns>
        public static string Truncate(this string @this, int maxLength, string suffix)
        {
            if (@this == null || @this.Length <= maxLength)
            {
                return @this;
            }
            return @this.Substring(0, maxLength - suffix.Length) + suffix;
        }

        /// <summary>
        /// EqualsIgnoreCase
        /// </summary>
        /// <param name="s1">string1</param>
        /// <param name="s2">string2</param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string s1, string s2)
            => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// string=>long
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static long? ToLong(this string @this)
        {
            long result;
            bool status = long.TryParse(@this, out result);

            //return status ? result : null;

            if (status)
                return result;
            else
                return null;
        }
    }
}
