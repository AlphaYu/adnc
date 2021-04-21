using System;

namespace Adnc.Infra.Common.Extensions
{
    public static class ByteExtension
    {
        /// <summary>
        ///     Returns the larger of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        /// <returns>Parameter  or , whichever is larger.</returns>
        public static byte Max(this byte val1, byte val2)
        {
            return Math.Max(val1, val2);
        }

        /// <summary>
        ///     Returns the smaller of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller.</returns>
        public static byte Min(this byte val1, byte val2)
        {
            return Math.Min(val1, val2);
        }
    }
}
