namespace System;

public static class ShortExtension
{
    /// <summary>
    ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
    public static bool InRange(this short @this, short minValue, short maxValue)
    {
        return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
    }

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
    ///     An Int16 extension method that query if '@this' is prime.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if prime, false if not.</returns>
    public static bool IsPrime(this short @this)
    {
        if (@this == 1 || @this == 2)
        {
            return true;
        }

        if (@this % 2 == 0)
        {
            return false;
        }

        var sqrt = (short)Math.Sqrt(@this);
        for (long t = 3; t <= sqrt; t = t + 2)
        {
            if (@this % t == 0)
            {
                return false;
            }
        }

        return true;
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

    /// <summary>
    ///     Converts a short value from host byte order to network byte order.
    /// </summary>
    /// <param name="host">The number to convert, expressed in host byte order.</param>
    /// <returns>A short value, expressed in network byte order.</returns>
    public static short HostToNetworkOrder(this short host)
    {
        return IPAddress.HostToNetworkOrder(host);
    }

    /// <summary>
    ///     Converts a short value from network byte order to host byte order.
    /// </summary>
    /// <param name="network">The number to convert, expressed in network byte order.</param>
    /// <returns>A short value, expressed in host byte order.</returns>
    public static short NetworkToHostOrder(this short network)
    {
        return IPAddress.NetworkToHostOrder(network);
    }
}