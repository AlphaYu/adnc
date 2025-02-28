namespace System
{
    public static class IntExtension
    {
        /// <summary>
        ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
        public static bool InRange(this int @this, int minValue, int maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        /// <summary>
        ///     An Int32 extension method that factor of.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="factorNumer">The factor numer.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool FactorOf(this int @this, int factorNumer)
        {
            return factorNumer % @this == 0;
        }

        /// <summary>
        ///     An Int32 extension method that query if '@this' is even.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if even, false if not.</returns>
        public static bool IsEven(this int @this)
        {
            return @this % 2 == 0;
        }

        /// <summary>
        ///     An Int32 extension method that query if '@this' is odd.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if odd, false if not.</returns>
        public static bool IsOdd(this int @this)
        {
            return @this % 2 != 0;
        }

        /// <summary>
        ///     An Int32 extension method that query if '@this' is multiple of.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>true if multiple of, false if not.</returns>
        public static bool IsMultipleOf(this int @this, int factor)
        {
            return @this % factor == 0;
        }

        /// <summary>
        ///     An Int32 extension method that query if '@this' is prime.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if prime, false if not.</returns>
        public static bool IsPrime(this int @this)
        {
            if (@this == 1 || @this == 2)
            {
                return true;
            }

            if (@this % 2 == 0)
            {
                return false;
            }

            var sqrt = (int)Math.Sqrt(@this);
            for (var t = 3; t <= sqrt; t = t + 2)
            {
                if (@this % t == 0)
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

        /// <summary>
        ///     Converts the specified Unicode code point into a UTF-16 encoded string.
        /// </summary>
        /// <param name="utf32">A 21-bit Unicode code point.</param>
        /// <returns>
        ///     A string consisting of one  object or a surrogate pair of  objects equivalent to the code point specified by
        ///     the  parameter.
        /// </returns>
        public static string ConvertFromUtf32(this int utf32)
        {
            return char.ConvertFromUtf32(utf32);
        }
    }
}