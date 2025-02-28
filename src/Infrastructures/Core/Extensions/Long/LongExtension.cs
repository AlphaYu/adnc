namespace System;

public static class LongExtension
{
    /// <summary>
    ///     Returns the specified 64-bit signed integer value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 8.</returns>
    public static byte[] GetBytes(this long value) => BitConverter.GetBytes(value);

    /// <summary>
    ///     Converts the specified 64-bit signed integer to a double-precision floating point number.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>A double-precision floating point number whose value is equivalent to .</returns>
    public static double Int64BitsToDouble(this long value) => BitConverter.Int64BitsToDouble(value);
}