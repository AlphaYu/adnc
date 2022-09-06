namespace System;

public static class ShortExtension
{

    /// <summary>
    ///     An Int16 extension method that factor of.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="factorNumer">The factor numer.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool FactorOf(this short @this, short factorNumer)
    {
        return factorNumer % @this == 0;
    }

    /// <summary>
    ///     An Int16 extension method that query if '@this' is even.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if even, false if not.</returns>
    public static bool IsEven(this short @this)
    {
        return @this % 2 == 0;
    }

    /// <summary>
    ///     An Int16 extension method that query if '@this' is odd.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if odd, false if not.</returns>
    public static bool IsOdd(this short @this)
    {
        return @this % 2 != 0;
    }

    /// <summary>
    ///     Returns the specified 16-bit signed integer value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 2.</returns>
    public static byte[] GetBytes(this short value)
    {
        return BitConverter.GetBytes(value);
    }
}