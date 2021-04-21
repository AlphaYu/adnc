using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace Adnc.Infr.Common.Extensions
{
    public static class ByteArray
    {
        /// <summary>
        ///     Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with
        ///     base-64 digits.
        /// </summary>
        /// <param name="inArray">An array of 8-bit unsigned integers.</param>
        /// <returns>The string representation, in base 64, of the contents of .</returns>
        public static string ToBase64String([NotNull] this byte[] inArray)
        {
            return Convert.ToBase64String(inArray);
        }

        /// <summary>
        ///     Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with
        ///     base-64 digits. A parameter specifies whether to insert line breaks in the return value.
        /// </summary>
        /// <param name="inArray">An array of 8-bit unsigned integers.</param>
        /// <param name="options">to insert a line break every 76 characters, or  to not insert line breaks.</param>
        /// <returns>The string representation in base 64 of the elements in .</returns>
        public static string ToBase64String([NotNull] this byte[] inArray, Base64FormattingOptions options)
        {
            return Convert.ToBase64String(inArray, options);
        }

        /// <summary>
        ///     Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation that is
        ///     encoded with base-64 digits. Parameters specify the subset as an offset in the input array, and the number of
        ///     elements in the array to convert.
        /// </summary>
        /// <param name="inArray">An array of 8-bit unsigned integers.</param>
        /// <param name="offset">An offset in .</param>
        /// <param name="length">The number of elements of  to convert.</param>
        /// <returns>The string representation in base 64 of  elements of , starting at position .</returns>
        public static string ToBase64String([NotNull] this byte[] inArray, int offset, int length)
        {
            return Convert.ToBase64String(inArray, offset, length);
        }

        /// <summary>
        ///     Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation that is
        ///     encoded with base-64 digits. Parameters specify the subset as an offset in the input array, the number of
        ///     elements in the array to convert, and whether to insert line breaks in the return value.
        /// </summary>
        /// <param name="inArray">An array of 8-bit unsigned integers.</param>
        /// <param name="offset">An offset in .</param>
        /// <param name="length">The number of elements of  to convert.</param>
        /// <param name="options">to insert a line break every 76 characters, or  to not insert line breaks.</param>
        /// <returns>The string representation in base 64 of  elements of , starting at position .</returns>
        public static string ToBase64String([NotNull] this byte[] inArray, int offset, int length, Base64FormattingOptions options)
        {
            return Convert.ToBase64String(inArray, offset, length, options);
        }

        /// <summary>
        ///     A byte[] extension method that resizes the byte[].
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="newSize">Size of the new.</param>
        /// <returns>A byte[].</returns>
        public static byte[] Resize([NotNull] this byte[] @this, int newSize)
        {
            Array.Resize(ref @this, newSize);
            return @this;
        }

        /// <summary>
        ///     A byte[] extension method that converts the @this byteArray to a memory stream.
        /// </summary>
        /// <param name="byteArray">The byetArray to act on</param>
        /// <returns>@this as a MemoryStream.</returns>
        public static MemoryStream ToMemoryStream([NotNull] this byte[] byteArray)
        {
            return new MemoryStream(byteArray);
        }

        public static string GetString([NotNull] this byte[] byteArray)
            => byteArray.GetString(Encoding.UTF8);

        public static string GetString([NotNull] this byte[] byteArray, Encoding encoding) => encoding.GetString(byteArray);
    }
}
