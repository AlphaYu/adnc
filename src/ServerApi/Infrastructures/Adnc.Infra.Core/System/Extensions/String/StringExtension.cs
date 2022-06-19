using System.Globalization;
using System.Text.RegularExpressions;

namespace System;

public static class StringExtension
{
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
    public static bool IsNotNullOrEmpty(this string @this) => !string.IsNullOrEmpty(@this);

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
        return @this[startIndex..];
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

    public static byte[] GetBytes([NotNull] this string str, Encoding encoding) => encoding.GetBytes(str);

    /// <summary>
    ///     A string extension method that converts the @this to an enum.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a T.</returns>
    public static T ToEnum<T>([NotNull] this string @this) => (T)Enum.Parse(typeof(T), @this);


    /// <summary>
    /// EqualsIgnoreCase
    /// </summary>
    /// <param name="s1">string1</param>
    /// <param name="s2">string2</param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(this string s1, string s2)   => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// string=>long
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public static long? ToLong(this string @this)
    {
        bool status = long.TryParse(@this, out long result);

        if (status)
            return result;
        else
            return null;
    }
}