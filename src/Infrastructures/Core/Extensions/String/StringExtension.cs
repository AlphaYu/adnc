using System.Text.RegularExpressions;

namespace System;

public static class StringExtension
{
    /// <summary>
    ///     A string extension method that query if 'value' is null or empty.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>true if null or empty, false if not.</returns>
    public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);

    /// <summary>
    ///     A string extension method that query if 'value' is not null and not empty.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>false if null or empty, true if not.</returns>
    public static bool IsNotNullOrEmpty(this string? value) => !string.IsNullOrEmpty(value);

    /// <summary>
    ///     A string extension method that query if 'value' is null or whiteSpace.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>true if null or whiteSpace, false if not.</returns>
    public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    ///     A string extension method that query if 'value' is not null and not whiteSpace.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>false if null or whiteSpace, true if not.</returns>
    public static bool IsNotNullOrWhiteSpace(this string? value) => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    ///     A string extension method that query if 'value' satisfy the specified pattern.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <param name="pattern">The pattern to use. Use '*' as wildcard string.</param>
    /// <returns>true if 'value' satisfy the specified pattern, false if not.</returns>
    public static bool IsLike([NotNull] this string value, string pattern)
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

        return Regex.IsMatch(value, regexPattern);
    }

    /// <summary>
    ///     A string extension method that repeats the string a specified number of times.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <param name="repeatCount">Number of repeats.</param>
    /// <returns>The repeated string.</returns>
    public static string Repeat([NotNull] this string value, int repeatCount)
    {
        if (value.Length == 1)
        {
            return new string(value[0], repeatCount);
        }

        var sb = new StringBuilder(repeatCount * value.Length);
        while (repeatCount-- > 0)
        {
            sb.Append(value);
        }

        return sb.ToString();
    }

    /// <summary>
    ///     A string extension method that reverses the given string.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>The string reversed.</returns>
    public static string Reverse([NotNull] this string value)
    {
        if (value.Length <= 1)
        {
            return value;
        }

        var chars = value.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    public static byte[] GetBytes([NotNull] this string str, Encoding encoding) => encoding.GetBytes(str);

    /// <summary>
    ///     A string extension method that converts the value to an enum.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="value">The value to act on.</param>
    /// <returns>value as a T.</returns>
    public static T ToEnum<T>([NotNull] this string value) => (T)Enum.Parse(typeof(T), value);

    /// <summary>
    /// EqualsIgnoreCase
    /// </summary>
    /// <param name="s1">string1</param>
    /// <param name="s2">string2</param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(this string s1, string s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// string=>long
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long? ToLong(this string value)
    {
        var status = long.TryParse(value, out var result);

        if (status)
        {
            return result;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// hex string to byte extension
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static byte[] ToBytes(this string hex)
    {
        if (hex.Length == 0)
        {
            return [0];
        }
        if (hex.Length % 2 == 1)
        {
            hex = "0" + hex;
        }
        var result = new byte[hex.Length / 2];
        for (var i = 0; i < hex.Length / 2; i++)
        {
            result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        }
        return result;
    }
}
