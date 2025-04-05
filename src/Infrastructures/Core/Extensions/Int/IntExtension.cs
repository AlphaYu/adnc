namespace System;

public static class IntExtension
{
    /// <summary>
    ///     An Int32 extension method that query if 'value' is even.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>true if even, false if not.</returns>
    public static bool IsEven(this int value)
    {
        return value % 2 == 0;
    }

    /// <summary>
    ///     An Int32 extension method that query if 'value' is odd.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>true if odd, false if not.</returns>
    public static bool IsOdd(this int value)
    {
        return value % 2 != 0;
    }

    /// <summary>
    ///     An Int32 extension method that query if 'value' is prime.
    /// </summary>
    /// <param name="value">The value to act on.</param>
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
}
