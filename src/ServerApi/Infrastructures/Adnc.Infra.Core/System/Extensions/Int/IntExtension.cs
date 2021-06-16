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

        /// <summary>
        ///     Returns the number of days in the specified month and year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month (a number ranging from 1 to 12).</param>
        /// <returns>
        ///     The number of days in  for the specified .For example, if  equals 2 for February, the return value is 28 or
        ///     29 depending upon whether  is a leap year.
        /// </returns>
        public static int DaysInMonth(this int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        /// <summary>
        ///     Returns an indication whether the specified year is a leap year.
        /// </summary>
        /// <param name="year">A 4-digit year.</param>
        /// <returns>true if  is a leap year; otherwise, false.</returns>
        public static bool IsLeapYear(this int year)
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
        ///     Returns the absolute value of a 32-bit signed integer.
        /// </summary>
        /// <param name="value">A number that is greater than , but less than or equal to .</param>
        /// <returns>A 32-bit signed integer, x, such that 0 ? x ?.</returns>
        public static int Abs(this int value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        ///     Produces the full product of two 32-bit numbers.
        /// </summary>
        /// <param name="a">The first number to multiply.</param>
        /// <param name="b">The second number to multiply.</param>
        /// <returns>The number containing the product of the specified numbers.</returns>
        public static long BigMul(this int a, int b)
        {
            return Math.BigMul(a, b);
        }

        /// <summary>
        ///     An Int32 extension method that div rem.
        /// </summary>
        /// <param name="a">a to act on.</param>
        /// <param name="b">The Int32 to process.</param>
        /// <param name="result">[out] The result.</param>
        /// <returns>An Int32.</returns>
        public static int DivRem(this int a, int b, out int result)
        {
            return Math.DivRem(a, b, out result);
        }

        /// <summary>
        ///     Returns the larger of two 32-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 32-bit signed integers to compare.</param>
        /// <returns>Parameter  or , whichever is larger.</returns>
        public static int Max(this int val1, int val2)
        {
            return Math.Max(val1, val2);
        }

        /// <summary>
        ///     Returns the smaller of two 32-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 32-bit signed integers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller.</returns>
        public static int Min(this int val1, int val2)
        {
            return Math.Min(val1, val2);
        }

        /// <summary>
        ///     Returns a value indicating the sign of a 32-bit signed integer.
        /// </summary>
        /// <param name="value">A signed number.</param>
        /// <returns>
        ///     A number that indicates the sign of , as shown in the following table.Return value Meaning -1  is less than
        ///     zero. 0  is equal to zero. 1  is greater than zero.
        /// </returns>
        public static int Sign(this int value)
        {
            return Math.Sign(value);
        }
    }
}