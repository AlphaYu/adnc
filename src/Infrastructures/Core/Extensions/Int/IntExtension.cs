namespace System;

public static class IntExtension
{
    /// <summary>
    ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
    /// </summary>
    /// <param name="this">The value to act on.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
    public static bool InRange(this int value, int minValue, int maxValue)
    {
        return value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0;
    }

    /// <summary>
    ///     An Int32 extension method that factor of.
    /// </summary>
    /// <param name="this">The value to act on.</param>
    /// <param name="factorNumer">The factor numer.</param>
    /// <returns>true if it succeeds, false if it fails.</returns>
    public static bool FactorOf(this int value, int factorNumer)
    {
        return factorNumer % value == 0;
    }

    /// <summary>
    ///     An Int32 extension method that query if 'value' is even.
    /// </summary>
    /// <param name="this">The value to act on.</param>
    /// <returns>true if even, false if not.</returns>
    public static bool IsEven(this int value)
    {
        return value % 2 == 0;
    }

    /// <summary>
    ///     An Int32 extension method that query if 'value' is odd.
    /// </summary>
    /// <param name="this">The value to act on.</param>
    /// <returns>true if odd, false if not.</returns>
    public static bool IsOdd(this int value)
    {
        return value % 2 != 0;
    }

    /// <summary>
    ///     An Int32 extension method that query if 'value' is multiple of.
    /// </summary>
    /// <param name="this">The value to act on.</param>
    /// <param name="factor">The factor.</param>
    /// <returns>true if multiple of, false if not.</returns>
    public static bool IsMultipleOf(this int value, int factor)
    {
        return value % factor == 0;
    }

    /// <summary>
    ///     An Int32 extension method that query if 'value' is prime.
    /// </summary>
    /// <param name="this">The value to act on.</param>
    /// <returns>true if prime, false if not.</returns>
    public static bool IsPrime(this int value)
    {
        if (value == 1 || value == 2)
        {
            return true;
        }

        if (value % 2 == 0)
        {
            return false;
        }

        var sqrt = (int)Math.Sqrt(value);
        for (var t = 3; t <= sqrt; t += 2)
        {
            if (value % t == 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///     Returns the specified 32-bit signed integer value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 4.</returns>
    public static byte[] GetBytes(this int value)
    {
        return BitConverter.GetBytes(value);
    }
}
