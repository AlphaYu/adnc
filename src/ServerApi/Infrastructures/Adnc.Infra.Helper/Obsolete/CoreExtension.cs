using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace Adnc.Common.Obsolete
{
    public static class CoreExtension
    {
        #region Array

        /// <summary>
        ///     An Array extension method that clears the array.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void ClearAll([NotNull] this Array @this)
        {
            Array.Clear(@this, 0, @this.Length);
        }

        /// <summary>
        ///     Searches an entire one-dimensional sorted  for a specific element, using the  interface implemented by each
        ///     element of the  and by the specified object.
        /// </summary>
        /// <param name="array">The sorted one-dimensional  to search.</param>
        /// <param name="value">The object to search for.</param>
        /// <returns>
        ///     The index of the specified  in the specified , if  is found. If  is not found and  is less than one or more
        ///     elements in , a negative number which is the bitwise complement of the index of the first element that is
        ///     larger than . If  is not found and  is greater than any of the elements in , a negative number which is the
        ///     bitwise complement of (the index of the last element plus 1).
        /// </returns>
        public static int BinarySearch([NotNull] this Array array, object value)
        {
            return Array.BinarySearch(array, value);
        }

        /// <summary>
        ///     Searches a range of elements in a one-dimensional sorted  for a value, using the  interface implemented by
        ///     each element of the  and by the specified value.
        /// </summary>
        /// <param name="array">The sorted one-dimensional  to search.</param>
        /// <param name="index">The starting index of the range to search.</param>
        /// <param name="length">The length of the range to search.</param>
        /// <param name="value">The object to search for.</param>
        /// <returns>
        ///     The index of the specified  in the specified , if  is found. If  is not found and  is less than one or more
        ///     elements in , a negative number which is the bitwise complement of the index of the first element that is
        ///     larger than . If  is not found and  is greater than any of the elements in , a negative number which is the
        ///     bitwise complement of (the index of the last element plus 1).
        /// </returns>
        public static int BinarySearch([NotNull] this Array array, int index, int length, object value)
        {
            return Array.BinarySearch(array, index, length, value);
        }

        /// <summary>
        ///     Searches an entire one-dimensional sorted  for a value using the specified  interface.
        /// </summary>
        /// <param name="array">The sorted one-dimensional  to search.</param>
        /// <param name="value">The object to search for.</param>
        /// <param name="comparer">
        ///     The  implementation to use when comparing elements.-or- null to use the  implementation
        ///     of each element.
        /// </param>
        /// <returns>
        ///     The index of the specified  in the specified , if  is found. If  is not found and  is less than one or more
        ///     elements in , a negative number which is the bitwise complement of the index of the first element that is
        ///     larger than . If  is not found and  is greater than any of the elements in , a negative number which is the
        ///     bitwise complement of (the index of the last element plus 1).
        /// </returns>
        public static int BinarySearch([NotNull] this Array array, object value, IComparer comparer)
        {
            return Array.BinarySearch(array, value, comparer);
        }

        /// <summary>
        ///     Searches a range of elements in a one-dimensional sorted  for a value, using the specified  interface.
        /// </summary>
        /// <param name="array">The sorted one-dimensional  to search.</param>
        /// <param name="index">The starting index of the range to search.</param>
        /// <param name="length">The length of the range to search.</param>
        /// <param name="value">The object to search for.</param>
        /// <param name="comparer">
        ///     The  implementation to use when comparing elements.-or- null to use the  implementation
        ///     of each element.
        /// </param>
        /// <returns>
        ///     The index of the specified  in the specified , if  is found. If  is not found and  is less than one or more
        ///     elements in , a negative number which is the bitwise complement of the index of the first element that is
        ///     larger than . If  is not found and  is greater than any of the elements in , a negative number which is the
        ///     bitwise complement of (the index of the last element plus 1).
        /// </returns>
        public static int BinarySearch([NotNull] this Array array, int index, int length, object value, IComparer comparer)
        {
            return Array.BinarySearch(array, index, length, value, comparer);
        }

        /// <summary>
        ///     Sets a range of elements in the  to zero, to false, or to null, depending on the element type.
        /// </summary>
        /// <param name="array">The  whose elements need to be cleared.</param>
        /// <param name="index">The starting index of the range of elements to clear.</param>
        /// <param name="length">The number of elements to clear.</param>
        public static void Clear([NotNull] this Array array, int index, int length)
        {
            Array.Clear(array, index, length);
        }

        /// <summary>
        ///     Copies a range of elements from an  starting at the first element and pastes them into another  starting at
        ///     the first element. The length is specified as a 32-bit integer.
        /// </summary>
        /// <param name="sourceArray">The  that contains the data to copy.</param>
        /// <param name="destinationArray">The  that receives the data.</param>
        /// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
        public static void Copy([NotNull] this Array sourceArray, Array destinationArray, int length)
        {
            Array.Copy(sourceArray, destinationArray, length);
        }

        /// <summary>
        ///     Copies a range of elements from an  starting at the specified source index and pastes them to another
        ///     starting at the specified destination index. The length and the indexes are specified as 32-bit integers.
        /// </summary>
        /// <param name="sourceArray">The  that contains the data to copy.</param>
        /// <param name="sourceIndex">A 32-bit integer that represents the index in the  at which copying begins.</param>
        /// <param name="destinationArray">The  that receives the data.</param>
        /// <param name="destinationIndex">A 32-bit integer that represents the index in the  at which storing begins.</param>
        /// <param name="length">A 32-bit integer that represents the number of elements to copy.</param>
        public static void Copy([NotNull] this Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
        {
            Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
        }

        /// <summary>
        ///     Copies a range of elements from an  starting at the first element and pastes them into another  starting at
        ///     the first element. The length is specified as a 64-bit integer.
        /// </summary>
        /// <param name="sourceArray">The  that contains the data to copy.</param>
        /// <param name="destinationArray">The  that receives the data.</param>
        /// <param name="length">
        ///     A 64-bit integer that represents the number of elements to copy. The integer must be between
        ///     zero and , inclusive.
        /// </param>
        public static void Copy([NotNull] this Array sourceArray, Array destinationArray, long length)
        {
            Array.Copy(sourceArray, destinationArray, length);
        }

        /// <summary>
        ///     Copies a range of elements from an  starting at the specified source index and pastes them to another
        ///     starting at the specified destination index. The length and the indexes are specified as 64-bit integers.
        /// </summary>
        /// <param name="sourceArray">The  that contains the data to copy.</param>
        /// <param name="sourceIndex">A 64-bit integer that represents the index in the  at which copying begins.</param>
        /// <param name="destinationArray">The  that receives the data.</param>
        /// <param name="destinationIndex">A 64-bit integer that represents the index in the  at which storing begins.</param>
        /// <param name="length">
        ///     A 64-bit integer that represents the number of elements to copy. The integer must be between
        ///     zero and , inclusive.
        /// </param>
        public static void Copy([NotNull] this Array sourceArray, long sourceIndex, Array destinationArray, long destinationIndex, long length)
        {
            Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
        }

        /// <summary>
        ///     Searches for the specified object and returns the index of the first occurrence within the entire one-
        ///     dimensional .
        /// </summary>
        /// <param name="array">The one-dimensional  to search.</param>
        /// <param name="value">The object to locate in .</param>
        /// <returns>
        ///     The index of the first occurrence of  within the entire , if found; otherwise, the lower bound of the array
        ///     minus 1.
        /// </returns>
        public static int IndexOf([NotNull] this Array array, object value)
        {
            return Array.IndexOf(array, value);
        }

        /// <summary>
        ///     Searches for the specified object and returns the index of the first occurrence within the range of elements
        ///     in the one-dimensional  that extends from the specified index to the last element.
        /// </summary>
        /// <param name="array">The one-dimensional  to search.</param>
        /// <param name="value">The object to locate in .</param>
        /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
        /// <returns>
        ///     The index of the first occurrence of  within the range of elements in  that extends from  to the last element,
        ///     if found; otherwise, the lower bound of the array minus 1.
        /// </returns>
        public static int IndexOf([NotNull] this Array array, object value, int startIndex)
        {
            return Array.IndexOf(array, value, startIndex);
        }

        /// <summary>
        ///     Searches for the specified object and returns the index of the first occurrence within the range of elements
        ///     in the one-dimensional  that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="array">The one-dimensional  to search.</param>
        /// <param name="value">The object to locate in .</param>
        /// <param name="startIndex">The starting index of the search. 0 (zero) is valid in an empty array.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>
        ///     The index of the first occurrence of  within the range of elements in  that starts at  and contains the
        ///     number of elements specified in , if found; otherwise, the lower bound of the array minus 1.
        /// </returns>
        public static int IndexOf([NotNull] this Array array, object value, int startIndex, int count)
        {
            return Array.IndexOf(array, value, startIndex, count);
        }

        /// <summary>
        ///     Searches for the specified object and returns the index of the last occurrence within the entire one-
        ///     dimensional .
        /// </summary>
        /// <param name="array">The one-dimensional  to search.</param>
        /// <param name="value">The object to locate in .</param>
        /// <returns>
        ///     The index of the last occurrence of  within the entire , if found; otherwise, the lower bound of the array
        ///     minus 1.
        /// </returns>
        public static int LastIndexOf([NotNull] this Array array, object value)
        {
            return Array.LastIndexOf(array, value);
        }

        /// <summary>
        ///     Searches for the specified object and returns the index of the last occurrence within the range of elements
        ///     in the one-dimensional  that extends from the first element to the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional  to search.</param>
        /// <param name="value">The object to locate in .</param>
        /// <param name="startIndex">The starting index of the backward search.</param>
        /// <returns>
        ///     The index of the last occurrence of  within the range of elements in  that extends from the first element to ,
        ///     if found; otherwise, the lower bound of the array minus 1.
        /// </returns>
        public static int LastIndexOf([NotNull] this Array array, object value, int startIndex)
        {
            return Array.LastIndexOf(array, value, startIndex);
        }

        /// <summary>
        ///     Searches for the specified object and returns the index of the last occurrence within the range of elements
        ///     in the one-dimensional  that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional  to search.</param>
        /// <param name="value">The object to locate in .</param>
        /// <param name="startIndex">The starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>
        ///     The index of the last occurrence of  within the range of elements in  that contains the number of elements
        ///     specified in  and ends at , if found; otherwise, the lower bound of the array minus 1.
        /// </returns>
        public static int LastIndexOf([NotNull] this Array array, object value, int startIndex, int count)
        {
            return Array.LastIndexOf(array, value, startIndex, count);
        }

        /// <summary>
        ///     Reverses the sequence of the elements in the entire one-dimensional .
        /// </summary>
        /// <param name="array">The one-dimensional  to reverse.</param>
        public static void Reverse([NotNull] this Array array)
        {
            Array.Reverse(array);
        }

        /// <summary>
        ///     Reverses the sequence of the elements in a range of elements in the one-dimensional .
        /// </summary>
        /// <param name="array">The one-dimensional  to reverse.</param>
        /// <param name="index">The starting index of the section to reverse.</param>
        /// <param name="length">The number of elements in the section to reverse.</param>
        public static void Reverse([NotNull] this Array array, int index, int length)
        {
            Array.Reverse(array, index, length);
        }

        /// <summary>
        ///     Sorts the elements in an entire one-dimensional  using the  implementation of each element of the .
        /// </summary>
        /// <param name="array">The one-dimensional  to sort.</param>
        public static void Sort([NotNull] this Array array)
        {
            Array.Sort(array);
        }

        /// <summary>
        ///     Sorts a pair of one-dimensional  objects (one contains the keys and the other contains the corresponding
        ///     items) based on the keys in the first  using the  implementation of each key.
        /// </summary>
        /// <param name="array">The one-dimensional  to sort.</param>
        /// <param name="items">
        ///     The one-dimensional  that contains the items that correspond to each of the keys in the .-or-
        ///     null to sort only the .
        /// </param>
        public static void Sort([NotNull] this Array array, Array items)
        {
            Array.Sort(array, items);
        }

        /// <summary>
        ///     Sorts the elements in a range of elements in a one-dimensional  using the  implementation of each element of
        ///     the .
        /// </summary>
        /// <param name="array">The one-dimensional  to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="length">The number of elements in the range to sort.</param>
        public static void Sort([NotNull] this Array array, int index, int length)
        {
            Array.Sort(array, index, length);
        }

        /// <summary>
        ///     Sorts a range of elements in a pair of one-dimensional  objects (one contains the keys and the other contains
        ///     the corresponding items) based on the keys in the first  using the  implementation of each key.
        /// </summary>
        /// <param name="array">The one-dimensional  to sort.</param>
        /// <param name="items">
        ///     The one-dimensional  that contains the items that correspond to each of the keys in the .-or-
        ///     null to sort only the .
        /// </param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="length">The number of elements in the range to sort.</param>
        public static void Sort([NotNull] this Array array, Array items, int index, int length)
        {
            Array.Sort(array, items, index, length);
        }

        /// <summary>
        ///     Sorts the elements in a one-dimensional  using the specified .
        /// </summary>
        /// <param name="array">The one-dimensional  to sort.</param>
        /// <param name="comparer">
        ///     The  implementation to use when comparing elements.-or-null to use the  implementation of
        ///     each element.
        /// </param>
        public static void Sort([NotNull] this Array array, IComparer comparer)
        {
            Array.Sort(array, comparer);
        }

        /// <summary>
        ///     Sorts a pair of one-dimensional  objects (one contains the keys and the other contains the corresponding
        ///     items) based on the keys in the first  using the specified .
        /// </summary>
        /// <param name="array">The one-dimensional  to sort.</param>
        /// <param name="items">
        ///     The one-dimensional  that contains the items that correspond to each of the keys in the .-or-
        ///     null to sort only the .
        /// </param>
        /// <param name="comparer">
        ///     The  implementation to use when comparing elements.-or-null to use the  implementation of
        ///     each element.
        /// </param>
        public static void Sort([NotNull] this Array array, Array items, IComparer comparer)
        {
            Array.Sort(array, items, comparer);
        }

        /// <summary>
        ///     Sorts the elements in a range of elements in a one-dimensional  using the specified .
        /// </summary>
        /// <param name="array">The one-dimensional  to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="length">The number of elements in the range to sort.</param>
        /// <param name="comparer">
        ///     The  implementation to use when comparing elements.-or-null to use the  implementation of
        ///     each element.
        /// </param>
        public static void Sort([NotNull] this Array array, int index, int length, IComparer comparer)
        {
            Array.Sort(array, index, length, comparer);
        }

        /// <summary>
        ///     Sorts a range of elements in a pair of one-dimensional  objects (one contains the keys and the other contains
        ///     the corresponding items) based on the keys in the first  using the specified .
        /// </summary>
        /// <param name="array">The one-dimensional  to sort.</param>
        /// <param name="items">
        ///     The one-dimensional  that contains the items that correspond to each of the keys in the .-or-
        ///     null to sort only the .
        /// </param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="length">The number of elements in the range to sort.</param>
        /// <param name="comparer">
        ///     The  implementation to use when comparing elements.-or-null to use the  implementation of
        ///     each element.
        /// </param>
        public static void Sort([NotNull] this Array array, Array items, int index, int length, IComparer comparer)
        {
            Array.Sort(array, items, index, length, comparer);
        }

        /// <summary>
        ///     Copies a specified number of bytes from a source array starting at a particular offset to a destination array
        ///     starting at a particular offset.
        /// </summary>
        /// <param name="src">The source buffer.</param>
        /// <param name="srcOffset">The zero-based byte offset into .</param>
        /// <param name="dst">The destination buffer.</param>
        /// <param name="dstOffset">The zero-based byte offset into .</param>
        /// <param name="count">The number of bytes to copy.</param>
        public static void BlockCopy([NotNull] this Array src, int srcOffset, Array dst, int dstOffset, int count)
        {
            Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
        }

        #endregion Array

        #region Boolean

        /// <summary>
        ///     A bool extension method that execute an Action if the value is true.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action to execute.</param>
        public static void IfTrue(this bool @this, Action action)
        {
            if (@this)
            {
                action();
            }
        }

        /// <summary>
        ///     A bool extension method that execute an Action if the value is false.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action to execute.</param>
        public static void IfFalse(this bool @this, Action action)
        {
            if (!@this)
            {
                action();
            }
        }

        /// <summary>
        ///     A bool extension method that show the trueValue when the @this value is true; otherwise show the falseValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="trueValue">The true value to be returned if the @this value is true.</param>
        /// <param name="falseValue">The false value to be returned if the @this value is false.</param>
        /// <returns>A string that represents of the current boolean value.</returns>
        public static string ToString(this bool @this, string trueValue, string falseValue)
        {
            return @this ? trueValue : falseValue;
        }

        #endregion Boolean

        #region Byte

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

        #endregion Byte

        #region ByteArray

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

        #endregion ByteArray

        #region Char

        /// <summary>
        ///     A char extension method that repeats a character the specified number of times.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="repeatCount">Number of repeats.</param>
        /// <returns>The repeated char.</returns>
        public static string Repeat(this char @this, int repeatCount)
        {
            return new string(@this, repeatCount);
        }

        /// <summary>
        ///     Converts the specified numeric Unicode character to a double-precision floating point number.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>The numeric value of  if that character represents a number; otherwise, -1.0.</returns>
        public static double GetNumericValue(this char c)
        {
            return char.GetNumericValue(c);
        }

        /// <summary>
        ///     Categorizes a specified Unicode character into a group identified by one of the  values.
        /// </summary>
        /// <param name="c">The Unicode character to categorize.</param>
        /// <returns>A  value that identifies the group that contains .</returns>
        public static UnicodeCategory GetUnicodeCategory(this char c)
        {
            return char.GetUnicodeCategory(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a control character.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a control character; otherwise, false.</returns>
        public static bool IsControl(this char c)
        {
            return char.IsControl(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a decimal digit.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a decimal digit; otherwise, false.</returns>
        public static bool IsDigit(this char c)
        {
            return char.IsDigit(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a Unicode letter.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a letter; otherwise, false.</returns>
        public static bool IsLetter(this char c)
        {
            return char.IsLetter(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a letter or a decimal digit.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a letter or a decimal digit; otherwise, false.</returns>
        public static bool IsLetterOrDigit(this char c)
        {
            return char.IsLetterOrDigit(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a lowercase letter.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a lowercase letter; otherwise, false.</returns>
        public static bool IsLower(this char c)
        {
            return char.IsLower(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as an uppercase letter.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is an uppercase letter; otherwise, false.</returns>
        public static bool IsUpper(this char c)
        {
            return char.IsUpper(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a number.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a number; otherwise, false.</returns>
        public static bool IsNumber(this char c)
        {
            return char.IsNumber(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a separator character.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a separator character; otherwise, false.</returns>
        public static bool IsSeparator(this char c)
        {
            return char.IsSeparator(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as a symbol character.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is a symbol character; otherwise, false.</returns>
        public static bool IsSymbol(this char c)
        {
            return char.IsSymbol(c);
        }

        /// <summary>
        ///     Indicates whether the specified Unicode character is categorized as white space.
        /// </summary>
        /// <param name="c">The Unicode character to evaluate.</param>
        /// <returns>true if  is white space; otherwise, false.</returns>
        public static bool IsWhiteSpace(this char c)
        {
            return char.IsWhiteSpace(c);
        }

        /// <summary>
        ///     Converts the value of a specified Unicode character to its lowercase equivalent using specified culture-
        ///     specific formatting information.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>
        ///     The lowercase equivalent of , modified according to , or the unchanged value of , if  is already lowercase or
        ///     not alphabetic.
        /// </returns>
        public static char ToLower(this char c, CultureInfo culture)
        {
            return char.ToLower(c, culture);
        }

        /// <summary>
        ///     Converts the value of a Unicode character to its lowercase equivalent.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>
        ///     The lowercase equivalent of , or the unchanged value of , if  is already lowercase or not alphabetic.
        /// </returns>
        public static char ToLower(this char c)
        {
            return char.ToLower(c);
        }

        /// <summary>
        ///     Converts the value of a Unicode character to its lowercase equivalent using the casing rules of the invariant
        ///     culture.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>
        ///     The lowercase equivalent of the  parameter, or the unchanged value of , if  is already lowercase or not
        ///     alphabetic.
        /// </returns>
        public static char ToLowerInvariant(this char c)
        {
            return char.ToLowerInvariant(c);
        }

        /// <summary>
        ///     Converts the value of a specified Unicode character to its uppercase equivalent using specified culture-
        ///     specific formatting information.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>
        ///     The uppercase equivalent of , modified according to , or the unchanged value of  if  is already uppercase,
        ///     has no uppercase equivalent, or is not alphabetic.
        /// </returns>
        public static char ToUpper(this char c, CultureInfo culture)
        {
            return char.ToUpper(c, culture);
        }

        /// <summary>
        ///     Converts the value of a Unicode character to its uppercase equivalent.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>
        ///     The uppercase equivalent of , or the unchanged value of  if  is already uppercase, has no uppercase
        ///     equivalent, or is not alphabetic.
        /// </returns>
        public static char ToUpper(this char c)
        {
            return char.ToUpper(c);
        }

        /// <summary>
        ///     Converts the value of a Unicode character to its uppercase equivalent using the casing rules of the invariant
        ///     culture.
        /// </summary>
        /// <param name="c">The Unicode character to convert.</param>
        /// <returns>
        ///     The uppercase equivalent of the  parameter, or the unchanged value of , if  is already uppercase or not
        ///     alphabetic.
        /// </returns>
        public static char ToUpperInvariant(this char c)
        {
            return char.ToUpperInvariant(c);
        }

        #endregion Char

        #region DateTime

        /// <summary>
        ///     A DateTime extension method that ages the given this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>An int.</returns>
        public static int Age(this DateTime @this)
        {
            if (DateTime.Today.Month < @this.Month ||
                DateTime.Today.Month == @this.Month &&
                DateTime.Today.Day < @this.Day)
            {
                return DateTime.Today.Year - @this.Year - 1;
            }
            return DateTime.Today.Year - @this.Year;
        }

        /// <summary>
        ///     A DateTime extension method that query if 'date' is date equal.
        /// </summary>
        /// <param name="date">The date to act on.</param>
        /// <param name="dateToCompare">Date/Time of the date to compare.</param>
        /// <returns>true if date equal, false if not.</returns>
        public static bool IsDateEqual(this DateTime date, DateTime dateToCompare) => date.Date == dateToCompare.Date;

        /// <summary>
        ///     A DateTime extension method that query if '@this' is today.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if today, false if not.</returns>
        public static bool IsToday(this DateTime @this)
        {
            return @this.Date == DateTime.Today;
        }

        /// <summary>
        ///     A DateTime extension method that query if '@this' is a week day.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if '@this' is a week day, false if not.</returns>
        public static bool IsWeekDay(this DateTime @this)
        {
            return !(@this.DayOfWeek == DayOfWeek.Saturday || @this.DayOfWeek == DayOfWeek.Sunday);
        }

        /// <summary>
        ///     A DateTime extension method that query if '@this' is a week day.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if '@this' is a week day, false if not.</returns>
        public static bool IsWeekendDay(this DateTime @this)
        {
            return @this.DayOfWeek == DayOfWeek.Saturday || @this.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        ///     A DateTime extension method that return a DateTime with the time set to "00:00:00:000". The first moment of
        ///     the day.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DateTime of the day with the time set to "00:00:00:000".</returns>
        public static DateTime StartOfDay(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, @this.Day);
        }

        /// <summary>
        ///     A DateTime extension method that return a DateTime of the first day of the month with the time set to
        ///     "00:00:00:000". The first moment of the first day of the month.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DateTime of the first day of the month with the time set to "00:00:00:000".</returns>
        public static DateTime StartOfMonth(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, 1);
        }

        /// <summary>
        ///     A DateTime extension method that starts of week.
        /// </summary>
        /// <param name="dt">The dt to act on.</param>
        /// <param name="startDayOfWeek">(Optional) the start day of week.</param>
        /// <returns>A DateTime.</returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startDayOfWeek = DayOfWeek.Sunday)
        {
            var start = new DateTime(dt.Year, dt.Month, dt.Day);

            if (start.DayOfWeek != startDayOfWeek)
            {
                var d = startDayOfWeek - start.DayOfWeek;
                if (startDayOfWeek <= start.DayOfWeek)
                {
                    return start.AddDays(d);
                }
                return start.AddDays(-7 + d);
            }

            return start;
        }

        /// <summary>
        ///     A DateTime extension method that return a DateTime of the first day of the year with the time set to
        ///     "00:00:00:000". The first moment of the first day of the year.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DateTime of the first day of the year with the time set to "00:00:00:000".</returns>
        public static DateTime StartOfYear(this DateTime @this)
        {
            return new DateTime(@this.Year, 1, 1);
        }

        /// <summary>
        ///     A DateTime extension method that converts the @this to an epoch time span.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a TimeSpan.</returns>
        public static TimeSpan ToEpochTimeSpan(this DateTime @this) => @this.ToUniversalTime().Subtract(new DateTime(1970, 1, 1));

        /// <summary>
        ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
        public static bool InRange(this DateTime @this, DateTime minValue, DateTime maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        /// <summary>
        ///     Converts a time to the time in a particular time zone.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <param name="destinationTimeZone">The time zone to convert  to.</param>
        /// <returns>The date and time in the destination time zone.</returns>
        public static DateTime ConvertTime(this DateTime dateTime, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, destinationTimeZone);
        }

        /// <summary>
        ///     Converts a time from one time zone to another.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <param name="sourceTimeZone">The time zone of .</param>
        /// <param name="destinationTimeZone">The time zone to convert  to.</param>
        /// <returns>
        ///     The date and time in the destination time zone that corresponds to the  parameter in the source time zone.
        /// </returns>
        public static DateTime ConvertTime(this DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, sourceTimeZone, destinationTimeZone);
        }

        /// <summary>
        ///     Converts a Coordinated Universal Time (UTC) to the time in a specified time zone.
        /// </summary>
        /// <param name="dateTime">The Coordinated Universal Time (UTC).</param>
        /// <param name="destinationTimeZone">The time zone to convert  to.</param>
        /// <returns>
        ///     The date and time in the destination time zone. Its  property is  if  is ; otherwise, its  property is .
        /// </returns>
        public static DateTime ConvertTimeFromUtc(this DateTime dateTime, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, destinationTimeZone);
        }

        /// <summary>
        ///     Converts the current date and time to Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <returns>
        ///     The Coordinated Universal Time (UTC) that corresponds to the  parameter. The  value&#39;s  property is always
        ///     set to .
        /// </returns>
        public static DateTime ConvertTimeToUtc(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime);
        }

        /// <summary>
        ///     Converts the time in a specified time zone to Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <param name="sourceTimeZone">The time zone of .</param>
        /// <returns>
        ///     The Coordinated Universal Time (UTC) that corresponds to the  parameter. The  object&#39;s  property is
        ///     always set to .
        /// </returns>
        public static DateTime ConvertTimeToUtc(this DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);
        }

        /// <summary>
        /// ToDateString("yyyy-MM-dd")
        /// </summary>
        /// <param name="this">dateTime</param>
        /// <returns></returns>
        public static string ToStandardDateString(this DateTime @this)
        {
            return @this.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// ToTimeString("yyyy-MM-dd HH:mm:ss")
        /// </summary>
        /// <param name="this">datetime</param>
        /// <returns></returns>
        public static string ToStandardTimeString(this DateTime @this)
        {
            return @this.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion DateTime

        #region Decimal

        /// <summary>
        ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
        public static bool InRange(this decimal @this, decimal minValue, decimal maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        /// <summary>
        ///     Returns the absolute value of a  number.
        /// </summary>
        /// <param name="value">A number that is greater than or equal to , but less than or equal to .</param>
        /// <returns>A decimal number, x, such that 0 ? x ?.</returns>
        public static decimal Abs(this decimal value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        ///     Returns the smallest integral value that is greater than or equal to the specified decimal number.
        /// </summary>
        /// <param name="d">A decimal number.</param>
        /// <returns>
        ///     The smallest integral value that is greater than or equal to . Note that this method returns a  instead of an
        ///     integral type.
        /// </returns>
        public static decimal Ceiling(this decimal d)
        {
            return Math.Ceiling(d);
        }

        /// <summary>
        ///     Returns the largest integer less than or equal to the specified decimal number.
        /// </summary>
        /// <param name="d">A decimal number.</param>
        /// <returns>
        ///     The largest integer less than or equal to .  Note that the method returns an integral value of type .
        /// </returns>
        public static decimal Floor(this decimal d)
        {
            return Math.Floor(d);
        }

        /// <summary>
        ///     Returns the larger of two decimal numbers.
        /// </summary>
        /// <param name="val1">The first of two decimal numbers to compare.</param>
        /// <param name="val2">The second of two decimal numbers to compare.</param>
        /// <returns>Parameter  or , whichever is larger.</returns>
        public static decimal Max(this decimal val1, decimal val2)
        {
            return Math.Max(val1, val2);
        }

        /// <summary>
        ///     Returns the smaller of two decimal numbers.
        /// </summary>
        /// <param name="val1">The first of two decimal numbers to compare.</param>
        /// <param name="val2">The second of two decimal numbers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller.</returns>
        public static decimal Min(this decimal val1, decimal val2)
        {
            return Math.Min(val1, val2);
        }

        /// <summary>
        ///     Rounds a decimal value to the nearest integral value.
        /// </summary>
        /// <param name="d">A decimal number to be rounded.</param>
        /// <returns>
        ///     The integer nearest parameter . If the fractional component of  is halfway between two integers, one of which
        ///     is even and the other odd, the even number is returned. Note that this method returns a  instead of an
        ///     integral type.
        /// </returns>
        public static decimal Round(this decimal d)
        {
            return Math.Round(d);
        }

        /// <summary>
        ///     Rounds a decimal value to a specified number of fractional digits.
        /// </summary>
        /// <param name="d">A decimal number to be rounded.</param>
        /// <param name="decimals">The number of decimal places in the return value.</param>
        /// <returns>The number nearest to  that contains a number of fractional digits equal to .</returns>
        public static decimal Round(this decimal d, int decimals)
        {
            return Math.Round(d, decimals);
        }

        /// <summary>
        ///     Rounds a decimal value to the nearest integer. A parameter specifies how to round the value if it is midway
        ///     between two numbers.
        /// </summary>
        /// <param name="d">A decimal number to be rounded.</param>
        /// <param name="mode">Specification for how to round  if it is midway between two other numbers.</param>
        /// <returns>
        ///     The integer nearest . If  is halfway between two numbers, one of which is even and the other odd, then
        ///     determines which of the two is returned.
        /// </returns>
        public static decimal Round(this decimal d, MidpointRounding mode)
        {
            return Math.Round(d, mode);
        }

        /// <summary>
        ///     Rounds a decimal value to a specified number of fractional digits. A parameter specifies how to round the
        ///     value if it is midway between two numbers.
        /// </summary>
        /// <param name="d">A decimal number to be rounded.</param>
        /// <param name="decimals">The number of decimal places in the return value.</param>
        /// <param name="mode">Specification for how to round  if it is midway between two other numbers.</param>
        /// <returns>
        ///     The number nearest to  that contains a number of fractional digits equal to . If  has fewer fractional digits
        ///     than ,  is returned unchanged.
        /// </returns>
        public static decimal Round(this decimal d, int decimals, MidpointRounding mode)
        {
            return Math.Round(d, decimals, mode);
        }

        /// <summary>
        ///     Returns a value indicating the sign of a decimal number.
        /// </summary>
        /// <param name="value">A signed decimal number.</param>
        /// <returns>
        ///     A number that indicates the sign of , as shown in the following table.Return value Meaning -1  is less than
        ///     zero. 0  is equal to zero. 1  is greater than zero.
        /// </returns>
        public static int Sign(this decimal value)
        {
            return Math.Sign(value);
        }

        /// <summary>
        ///     Calculates the integral part of a specified decimal number.
        /// </summary>
        /// <param name="d">A number to truncate.</param>
        /// <returns>
        ///     The integral part of ; that is, the number that remains after any fractional digits have been discarded.
        /// </returns>
        public static decimal Truncate(this decimal d)
        {
            return Math.Truncate(d);
        }

        /// <summary>
        ///     A Decimal extension method that converts the @this to a money.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a Decimal.</returns>
        public static decimal ToMoney(this decimal @this)
        {
            return Math.Round(@this, 2);
        }

        #endregion Decimal

        #region Delegate

        /// <summary>
        /// Concatenates the invocation lists of two delegates.
        /// </summary>
        /// <param name="a">The delegate whose invocation list comes first.</param>
        /// <param name="b">The delegate whose invocation list comes last.</param>
        /// <returns>
        ///     A new delegate with an invocation list that concatenates the invocation lists of  and  in that order. Returns
        ///     if  is null, returns  if  is a null reference, and returns a null reference if both  and  are null references.
        /// </returns>
        public static Delegate Combine([NotNull] this Delegate a, Delegate b)
        {
            return Delegate.Combine(a, b);
        }

        /// <summary>
        /// Removes the last occurrence of the invocation list of a delegate from the invocation list of another delegate.
        /// </summary>
        /// <param name="source">The delegate from which to remove the invocation list of .</param>
        /// <param name="value">The delegate that supplies the invocation list to remove from the invocation list of .</param>
        /// <returns>
        ///     A new delegate with an invocation list formed by taking the invocation list of  and removing the last
        ///     occurrence of the invocation list of , if the invocation list of  is found within the invocation list of .
        ///     Returns  if  is null or if the invocation list of  is not found within the invocation list of . Returns a
        ///     null reference if the invocation list of  is equal to the invocation list of  or if  is a null reference.
        /// </returns>
        public static Delegate Remove([NotNull] this Delegate source, Delegate value)
        {
            return Delegate.Remove(source, value);
        }

        /// <summary>
        /// Removes all occurrences of the invocation list of a delegate from the invocation list of another delegate.
        /// </summary>
        /// <param name="source">The delegate from which to remove the invocation list of .</param>
        /// <param name="value">The delegate that supplies the invocation list to remove from the invocation list of .</param>
        /// <returns>
        ///     A new delegate with an invocation list formed by taking the invocation list of  and removing all occurrences
        ///     of the invocation list of , if the invocation list of  is found within the invocation list of . Returns  if
        ///     is null or if the invocation list of  is not found within the invocation list of . Returns a null reference
        ///     if the invocation list of  is equal to the invocation list of , if  contains only a series of invocation
        ///     lists that are equal to the invocation list of , or if  is a null reference.
        /// </returns>
        public static Delegate RemoveAll([NotNull] this Delegate source, Delegate value)
        {
            return Delegate.RemoveAll(source, value);
        }

        #endregion Delegate

        #region Double

        /// <summary>
        /// A T extension method that check if the value is between inclusively the minValue and maxValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
        public static bool InRange(this double @this, double minValue, double maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        /// <summary>
        ///     Returns the absolute value of a double-precision floating-point number.
        /// </summary>
        /// <param name="value">A number that is greater than or equal to , but less than or equal to .</param>
        /// <returns>A double-precision floating-point number, x, such that 0 ? x ?.</returns>
        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        ///     Returns the angle whose cosine is the specified number.
        /// </summary>
        /// <param name="d">
        ///     A number representing a cosine, where  must be greater than or equal to -1, but less than or
        ///     equal to 1.
        /// </param>
        /// <returns>An angle, ?, measured in radians, such that 0 ????-or-  if  &lt; -1 or  &gt; 1 or  equals .</returns>
        public static double Acos(this double d)
        {
            return Math.Acos(d);
        }

        /// <summary>
        ///     Returns the angle whose sine is the specified number.
        /// </summary>
        /// <param name="d">
        ///     A number representing a sine, where  must be greater than or equal to -1, but less than or equal
        ///     to 1.
        /// </param>
        /// <returns>
        ///     An angle, ?, measured in radians, such that -?/2 ????/2 -or-  if  &lt; -1 or  &gt; 1 or  equals .
        /// </returns>
        public static double Asin(this double d)
        {
            return Math.Asin(d);
        }

        /// <summary>
        ///     Returns the angle whose tangent is the specified number.
        /// </summary>
        /// <param name="d">A number representing a tangent.</param>
        /// <returns>
        ///     An angle, ?, measured in radians, such that -?/2 ????/2.-or-  if  equals , -?/2 rounded to double precision (-
        ///     1.5707963267949) if  equals , or ?/2 rounded to double precision (1.5707963267949) if  equals .
        /// </returns>
        public static double Atan(this double d)
        {
            return Math.Atan(d);
        }

        /// <summary>
        ///     Returns the angle whose tangent is the quotient of two specified numbers.
        /// </summary>
        /// <param name="y">The y coordinate of a point.</param>
        /// <param name="x">The x coordinate of a point.</param>
        /// <returns>
        ///     An angle, ?, measured in radians, such that -?????, and tan(?) =  / , where (, ) is a point in the Cartesian
        ///     plane. Observe the following: For (, ) in quadrant 1, 0 &lt; ? &lt; ?/2.For (, ) in quadrant 2, ?/2 &lt;
        ///     ???.For (, ) in quadrant 3, -? &lt; ? &lt; -?/2.For (, ) in quadrant 4, -?/2 &lt; ? &lt; 0.For points on the
        ///     boundaries of the quadrants, the return value is the following:If y is 0 and x is not negative, ? = 0.If y is
        ///     0 and x is negative, ? = ?.If y is positive and x is 0, ? = ?/2.If y is negative and x is 0, ? = -?/2.If  or
        ///     is , or if  and  are either  or , the method returns .
        /// </returns>
        public static double Atan2(this double y, double x)
        {
            return Math.Atan2(y, x);
        }

        /// <summary>
        ///     Returns the smallest integral value that is greater than or equal to the specified double-precision floating-
        ///     point number.
        /// </summary>
        /// <param name="a">A double-precision floating-point number.</param>
        /// <returns>
        ///     The smallest integral value that is greater than or equal to . If  is equal to , , or , that value is
        ///     returned. Note that this method returns a  instead of an integral type.
        /// </returns>
        public static int Ceiling(this double a)
            => Convert.ToInt32(Math.Ceiling(a));

        /// <summary>
        ///     Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="d">An angle, measured in radians.</param>
        /// <returns>The cosine of . If  is equal to , , or , this method returns .</returns>
        public static double Cos(this double d)
        {
            return Math.Cos(d);
        }

        /// <summary>
        ///     Returns the hyperbolic cosine of the specified angle.
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The hyperbolic cosine of . If  is equal to  or ,  is returned. If  is equal to ,  is returned.</returns>
        public static double Cosh(this double value)
        {
            return Math.Cosh(value);
        }

        /// <summary>
        ///     Returns e raised to the specified power.
        /// </summary>
        /// <param name="d">A number specifying a power.</param>
        /// <returns>
        ///     The number e raised to the power . If  equals  or , that value is returned. If  equals , 0 is returned.
        /// </returns>
        public static double Exp(this double d)
        {
            return Math.Exp(d);
        }

        /// <summary>
        ///     Returns the largest integer less than or equal to the specified double-precision floating-point number.
        /// </summary>
        /// <param name="d">A double-precision floating-point number.</param>
        /// <returns>The largest integer less than or equal to . If  is equal to , , or , that value is returned.</returns>
        public static int Floor(this double d)
        {
            return Convert.ToInt32(Math.Floor(d));
        }

        /// <summary>
        ///     Returns the remainder resulting from the division of a specified number by another specified number.
        /// </summary>
        /// <param name="x">A dividend.</param>
        /// <param name="y">A divisor.</param>
        /// <returns>
        ///     A number equal to  - ( Q), where Q is the quotient of  /  rounded to the nearest integer (if  /  falls
        ///     halfway between two integers, the even integer is returned).If  - ( Q) is zero, the value +0 is returned if
        ///     is positive, or -0 if  is negative.If  = 0,  is returned.
        /// </returns>
        public static double IEEERemainder(this double x, double y)
        {
            return Math.IEEERemainder(x, y);
        }

        /// <summary>
        ///     Returns the natural (base e) logarithm of a specified number.
        /// </summary>
        /// <param name="d">The number whose logarithm is to be found.</param>
        /// <returns>
        ///     One of the values in the following table.  parameterReturn value Positive The natural logarithm of ; that is,
        ///     ln , or log eZero Negative Equal to Equal to.
        /// </returns>
        public static double Log(this double d)
        {
            return Math.Log(d);
        }

        /// <summary>
        ///     Returns the logarithm of a specified number in a specified base.
        /// </summary>
        /// <param name="d">The number whose logarithm is to be found.</param>
        /// <param name="newBase">The base of the logarithm.</param>
        /// <returns>
        ///     One of the values in the following table. (+Infinity denotes , -Infinity denotes , and NaN denotes .)Return
        ///     value&gt; 0(0 &lt;&lt; 1) -or-(&gt; 1)lognewBase(a)&lt; 0(any value)NaN(any value)&lt; 0NaN != 1 = 0NaN != 1
        ///     = +InfinityNaN = NaN(any value)NaN(any value) = NaNNaN(any value) = 1NaN = 00 &lt;&lt; 1 +Infinity = 0&gt; 1-
        ///     Infinity =  +Infinity0 &lt;&lt; 1-Infinity =  +Infinity&gt; 1+Infinity = 1 = 00 = 1 = +Infinity0.
        /// </returns>
        public static double Log(this double d, double newBase)
        {
            return Math.Log(d, newBase);
        }

        /// <summary>
        ///     Returns the base 10 logarithm of a specified number.
        /// </summary>
        /// <param name="d">A number whose logarithm is to be found.</param>
        /// <returns>
        ///     One of the values in the following table.  parameter Return value Positive The base 10 log of ; that is, log
        ///     10. Zero Negative Equal to Equal to.
        /// </returns>
        public static double Log10(this double d)
        {
            return Math.Log10(d);
        }

        /// <summary>
        ///     Returns the larger of two double-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
        /// <returns>Parameter  or , whichever is larger. If , , or both  and  are equal to ,  is returned.</returns>
        public static double Max(this double val1, double val2)
        {
            return Math.Max(val1, val2);
        }

        /// <summary>
        ///     Returns the smaller of two double-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller. If , , or both  and  are equal to ,  is returned.</returns>
        public static double Min(this double val1, double val2) => Math.Min(val1, val2);

        /// <summary>
        ///     Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="x">A double-precision floating-point number to be raised to a power.</param>
        /// <param name="y">A double-precision floating-point number that specifies a power.</param>
        /// <returns>The number  raised to the power .</returns>
        public static double Pow(this double x, double y) => Math.Pow(x, y);

        /// <summary>
        ///     Rounds a double-precision floating-point value to the nearest integral value.
        /// </summary>
        /// <param name="a">A double-precision floating-point number to be rounded.</param>
        /// <returns>
        ///     The integer nearest . If the fractional component of  is halfway between two integers, one of which is even
        ///     and the other odd, then the even number is returned. Note that this method returns a  instead of an integral
        ///     type.
        /// </returns>
        public static double Round(this double a) => Math.Round(a);

        /// <summary>
        ///     Rounds a double-precision floating-point value to a specified number of fractional digits.
        /// </summary>
        /// <param name="a">A double-precision floating-point number to be rounded.</param>
        /// <param name="digits">The number of fractional digits in the return value.</param>
        /// <returns>The number nearest to  that contains a number of fractional digits equal to .</returns>
        public static double Round(this double a, int digits) => Math.Round(a, digits);

        /// <summary>
        ///     Rounds a double-precision floating-point value to the nearest integer. A parameter specifies how to round the
        ///     value if it is midway between two numbers.
        /// </summary>
        /// <param name="a">A double-precision floating-point number to be rounded.</param>
        /// <param name="mode">Specification for how to round  if it is midway between two other numbers.</param>
        /// <returns>
        ///     The integer nearest . If  is halfway between two integers, one of which is even and the other odd, then
        ///     determines which of the two is returned.
        /// </returns>
        public static double Round(this double a, MidpointRounding mode) => Math.Round(a, mode);

        /// <summary>
        ///     Rounds a double-precision floating-point value to a specified number of fractional digits. A parameter
        ///     specifies how to round the value if it is midway between two numbers.
        /// </summary>
        /// <param name="value">A double-precision floating-point number to be rounded.</param>
        /// <param name="digits">The number of fractional digits in the return value.</param>
        /// <param name="mode">Specification for how to round  if it is midway between two other numbers.</param>
        /// <returns>
        ///     The number nearest to  that has a number of fractional digits equal to . If  has fewer fractional digits than
        ///     ,  is returned unchanged.
        /// </returns>
        public static double Round(this double value, int digits, MidpointRounding mode) => Math.Round(value, digits, mode);

        /// <summary>
        ///     Returns a value indicating the sign of a double-precision floating-point number.
        /// </summary>
        /// <param name="value">A signed number.</param>
        /// <returns>
        ///     A number that indicates the sign of , as shown in the following table.Return value Meaning -1  is less than
        ///     zero. 0  is equal to zero. 1  is greater than zero.
        /// </returns>
        public static int Sign(this double value) => Math.Sign(value);

        /// <summary>
        ///     Returns the sine of the specified angle.
        /// </summary>
        /// <param name="a">An angle, measured in radians.</param>
        /// <returns>The sine of . If  is equal to , , or , this method returns .</returns>
        public static double Sin(this double a)
        {
            return Math.Sin(a);
        }

        /// <summary>
        ///     Returns the hyperbolic sine of the specified angle.
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The hyperbolic sine of . If  is equal to , , or , this method returns a  equal to .</returns>
        public static double Sinh(this double value)
        {
            return Math.Sinh(value);
        }

        /// <summary>
        ///     Returns the square root of a specified number.
        /// </summary>
        /// <param name="d">The number whose square root is to be found.</param>
        /// <returns>
        ///     One of the values in the following table.  parameter Return value Zero or positive The positive square root
        ///     of . Negative Equals Equals.
        /// </returns>
        public static double Sqrt(this double d)
        {
            return Math.Sqrt(d);
        }

        /// <summary>
        ///     Returns the tangent of the specified angle.
        /// </summary>
        /// <param name="a">An angle, measured in radians.</param>
        /// <returns>The tangent of . If  is equal to , , or , this method returns .</returns>
        public static double Tan(this double a)
        {
            return Math.Tan(a);
        }

        /// <summary>
        ///     Returns the hyperbolic tangent of the specified angle.
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>
        ///     The hyperbolic tangent of . If  is equal to , this method returns -1. If value is equal to , this method
        ///     returns 1. If  is equal to , this method returns .
        /// </returns>
        public static double Tanh(this double value)
        {
            return Math.Tanh(value);
        }

        /// <summary>
        ///     Calculates the integral part of a specified double-precision floating-point number.
        /// </summary>
        /// <param name="d">A number to truncate.</param>
        /// <returns>
        ///     The integral part of ; that is, the number that remains after any fractional digits have been discarded, or
        ///     one of the values listed in the following table. Return value.
        /// </returns>
        public static double Truncate(this double d) => Math.Truncate(d);

        /// <summary>
        ///     A Double extension method that converts the @this to a moneyFormat.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a Double.</returns>
        public static double ToMoney(this double @this) => Math.Round(@this, 2);

        #endregion Double

        #region Enum

        /// <summary>
        /// A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        public static bool In([NotNull] this Enum @this, params Enum[] values)
        {
            return Array.IndexOf(values, @this) >= 0;
        }

        /// <summary>
        /// An object extension method that gets description attribute.
        /// </summary>
        /// <param name="value">The value to act on.</param>
        /// <returns>The description attribute.</returns>
        public static string GetDescription([NotNull] this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString())
                .GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description;
        }

        #endregion Enum

        #region EventHandler

        /// <summary>
        ///     An EventHandler extension method that raises the event event.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="sender">Source of the event.</param>
        public static void RaiseEvent([CanBeNull] this EventHandler @this, object sender)
        {
            @this?.Invoke(sender, null);
        }

        /// <summary>
        ///     An EventHandler extension method that raises.
        /// </summary>
        /// <param name="handler">The handler to act on.</param>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event information.</param>
        public static void RaiseEvent([CanBeNull] this EventHandler handler, object sender, EventArgs e)
        {
            handler?.Invoke(sender, e);
        }

        /// <summary>
        ///     An EventHandler&lt;TEventArgs&gt; extension method that raises the event event.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of the event arguments.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="sender">Source of the event.</param>
        public static void RaiseEvent<TEventArgs>([CanBeNull] this EventHandler<TEventArgs> @this, object sender) where TEventArgs : EventArgs
        {
            @this?.Invoke(sender, Activator.CreateInstance<TEventArgs>());
        }

        /// <summary>
        ///     An EventHandler&lt;TEventArgs&gt; extension method that raises the event event.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of the event arguments.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event information to send to registered event handlers.</param>
        public static void RaiseEvent<TEventArgs>([CanBeNull] this EventHandler<TEventArgs> @this, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            @this?.Invoke(sender, e);
        }

        #endregion EventHandler

        #region Guid

        /// <summary>A GUID extension method that query if '@this' is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsNullOrEmpty([NotNull] this Guid? @this)
        {
            return !@this.HasValue || @this == Guid.Empty;
        }

        /// <summary>A GUID extension method that query if '@this' is not null or empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsNotNullOrEmpty([NotNull] this Guid? @this)
        {
            return @this.HasValue && @this.Value != Guid.Empty;
        }

        /// <summary>A GUID extension method that query if '@this' is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if empty, false if not.</returns>
        public static bool IsEmpty(this Guid @this)
        {
            return @this == Guid.Empty;
        }

        /// <summary>A GUID extension method that queries if a not is empty.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if a not is empty, false if not.</returns>
        public static bool IsNotEmpty(this Guid @this)
        {
            return @this != Guid.Empty;
        }

        #endregion Guid

        #region short

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
        ///     Returns the larger of two 16-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 16-bit signed integers to compare.</param>
        /// <returns>Parameter  or , whichever is larger.</returns>
        public static short Max(this short val1, short val2)
        {
            return Math.Max(val1, val2);
        }

        /// <summary>
        ///     Returns the smaller of two 16-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 16-bit signed integers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller.</returns>
        public static short Min(this short val1, short val2)
        {
            return Math.Min(val1, val2);
        }

        /// <summary>
        ///     Returns a value indicating the sign of a 16-bit signed integer.
        /// </summary>
        /// <param name="value">A signed number.</param>
        /// <returns>
        ///     A number that indicates the sign of , as shown in the following table.Return value Meaning -1  is less than
        ///     zero. 0  is equal to zero. 1  is greater than zero.
        /// </returns>
        public static int Sign(this short value)
        {
            return Math.Sign(value);
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

        #endregion short

        #region int

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

        #endregion int

        #region long

        /// <summary>
        ///     Returns a  that represents a specified time, where the specification is in units of ticks.
        /// </summary>
        /// <param name="value">A number of ticks that represent a time.</param>
        /// <returns>An object that represents .</returns>
        public static TimeSpan FromTicks(this long value)
        {
            return TimeSpan.FromTicks(value);
        }

        /// <summary>
        ///     Returns the specified 64-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 8.</returns>
        public static byte[] GetBytes(this long value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        ///     Converts the specified 64-bit signed integer to a double-precision floating point number.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A double-precision floating point number whose value is equivalent to .</returns>
        public static double Int64BitsToDouble(this long value)
        {
            return BitConverter.Int64BitsToDouble(value);
        }

        #endregion long

        #region object

        /// <summary>
        ///     An object extension method that converts the @this to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A T.</returns>
        public static T AsOrDefault<T>([NotNull] this object @this)
        {
            try
            {
                return (T)@this;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        ///     An object extension method that converts the @this to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>A T.</returns>
        public static T AsOrDefault<T>([NotNull] this object @this, T defaultValue)
        {
            try
            {
                return (T)@this;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     An object extension method that converts the @this to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>A T.</returns>
        public static T AsOrDefault<T>([NotNull] this object @this, Func<T> defaultValueFactory)
        {
            try
            {
                return (T)@this;
            }
            catch (Exception)
            {
                return defaultValueFactory();
            }
        }

        /// <summary>
        ///     An object extension method that converts the @this to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>A T.</returns>
        public static T AsOrDefault<T>([NotNull] this object @this, Func<object, T> defaultValueFactory)
        {
            try
            {
                return (T)@this;
            }
            catch (Exception)
            {
                return defaultValueFactory(@this);
            }
        }

        /// <summary>
        ///     A System.Object extension method that toes the given this.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <returns>A T.</returns>
        public static T To<T>(this object @this)
        {
            if (@this == null)
            {
                return (T)(object)null;
            }

            var targetType = typeof(T);

            if (@this.GetType() == targetType)
            {
                return (T)@this;
            }
            var converter = TypeDescriptor.GetConverter(@this);
            if (converter != null)
            {
                if (converter.CanConvertTo(targetType))
                {
                    return (T)converter.ConvertTo(@this, targetType);
                }
            }

            converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null)
            {
                if (converter.CanConvertFrom(@this.GetType()))
                {
                    return (T)converter.ConvertFrom(@this);
                }
            }

            if (@this == DBNull.Value)
            {
                return (T)(object)null;
            }

            return (T)Convert.ChangeType(@this, targetType);
        }

        /// <summary>
        ///     A System.Object extension method that toes the given this.
        /// </summary>
        /// <param name="this">this.</param>
        /// <param name="type">The type.</param>
        /// <returns>An object.</returns>
        public static object To([CanBeNull] this object @this, Type type)
        {
            if (@this != null)
            {
                var targetType = type;

                if (@this.GetType() == targetType)
                {
                    return @this;
                }

                var converter = TypeDescriptor.GetConverter(@this);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                    {
                        return converter.ConvertTo(@this, targetType);
                    }
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(@this.GetType()))
                    {
                        return converter.ConvertFrom(@this);
                    }
                }

                if (@this == DBNull.Value)
                {
                    return null;
                }

                return Convert.ChangeType(@this, targetType);
            }

            return @this;
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a T.</returns>
        public static T ToOrDefault<T>([CanBeNull] this object @this, Func<object, T> defaultValueFactory)
        {
            try
            {
                return (T)@this.To(typeof(T));
            }
            catch (Exception)
            {
                return defaultValueFactory(@this);
            }
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <param name="defaultValueFactory">The default value factory.</param>
        /// <returns>The given data converted to a T.</returns>
        public static T ToOrDefault<T>([NotNull] this object @this, Func<T> defaultValueFactory)
        {
            return @this.ToOrDefault(x => defaultValueFactory());
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <param name="this">this.</param>
        /// <param name="type">type</param>
        /// <returns>The given data converted to</returns>
        public static object ToOrDefault([NotNull] this object @this, [NotNull] Type type)
        {
            try
            {
                return @this.To(type);
            }
            catch (Exception)
            {
                return type.GetDefaultValue();
            }
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <returns>The given data converted to a T.</returns>
        public static T ToOrDefault<T>([CanBeNull] this object @this)
        {
            return @this.ToOrDefault(x => default(T));
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a T.</returns>
        public static T ToOrDefault<T>([CanBeNull] this object @this, T defaultValue)
        {
            return @this.ToOrDefault(x => defaultValue);
        }

        /// <summary>
        ///     An object extension method that query if '@this' is assignable from.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if assignable from, false if not.</returns>
        public static bool IsAssignableFrom<T>([NotNull] this object @this)
        {
            var type = @this.GetType();
            return type.IsAssignableFrom(typeof(T));
        }

        /// <summary>
        ///     An object extension method that query if '@this' is assignable from.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>true if assignable from, false if not.</returns>
        public static bool IsAssignableFrom([NotNull] this object @this, Type targetType)
        {
            var type = @this.GetType();
            return type.IsAssignableFrom(targetType);
        }

        /// <summary>
        ///     A T extension method that chains actions.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action.</param>
        /// <returns>The @this acted on.</returns>
        public static T Chain<T>([NotNull] this T @this, Action<T> action)
        {
            action?.Invoke(@this);

            return @this;
        }

        /// <summary>
        ///     A T extension method that makes a deep copy of '@this' object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter. It should be <c>Serializable</c></typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>the copied object.</returns>
        public static T DeepClone<T>([NotNull] this T @this)
        {
            using (var stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, @this);
                formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        ///     A T extension method that null if.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A T.</returns>
        public static T NullIf<T>([NotNull] this T @this, Func<T, bool> predicate) where T : class
        {
            if (predicate(@this))
            {
                return null;
            }
            return @this;
        }

        /// <summary>
        ///     A T extension method that gets value or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="func">The function.</param>
        /// <returns>The value or default.</returns>
        public static TResult GetValueOrDefault<T, TResult>([NotNull] this T @this, Func<T, TResult> func)
        {
            try
            {
                return func(@this);
            }
            catch (Exception)
            {
                return default(TResult);
            }
        }

        /// <summary>
        ///     A T extension method that gets value or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="func">The function.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value or default.</returns>
        public static TResult GetValueOrDefault<T, TResult>([NotNull] this T @this, Func<T, TResult> func, TResult defaultValue)
        {
            try
            {
                return func(@this);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="catchValue">The catch value.</param>
        /// <returns>A TResult.</returns>
        public static TResult Try<TType, TResult>([NotNull] this TType @this, Func<TType, TResult> tryFunction, TResult catchValue)
        {
            try
            {
                return tryFunction(@this);
            }
            catch
            {
                return catchValue;
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="catchValueFactory">The catch value factory.</param>
        /// <returns>A TResult.</returns>
        public static TResult Try<TType, TResult>([NotNull] this TType @this, Func<TType, TResult> tryFunction, Func<TType, TResult> catchValueFactory)
        {
            try
            {
                return tryFunction(@this);
            }
            catch
            {
                return catchValueFactory(@this);
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="result">[out] The result.</param>
        /// <returns>A TResult.</returns>
        public static bool Try<TType, TResult>([NotNull] this TType @this, Func<TType, TResult> tryFunction, out TResult result)
        {
            try
            {
                result = tryFunction(@this);
                return true;
            }
            catch
            {
                result = default(TResult);
                return false;
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="catchValue">The catch value.</param>
        /// <param name="result">[out] The result.</param>
        /// <returns>A TResult.</returns>
        public static bool Try<TType, TResult>([NotNull] this TType @this, Func<TType, TResult> tryFunction, TResult catchValue, out TResult result)
        {
            try
            {
                result = tryFunction(@this);
                return true;
            }
            catch
            {
                result = catchValue;
                return false;
            }
        }

        /// <summary>A TType extension method that tries.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryFunction">The try function.</param>
        /// <param name="catchValueFactory">The catch value factory.</param>
        /// <param name="result">[out] The result.</param>
        /// <returns>A TResult.</returns>
        public static bool Try<TType, TResult>([NotNull] this TType @this, Func<TType, TResult> tryFunction, Func<TType, TResult> catchValueFactory, out TResult result)
        {
            try
            {
                result = tryFunction(@this);
                return true;
            }
            catch
            {
                result = catchValueFactory(@this);
                return false;
            }
        }

        /// <summary>A TType extension method that attempts to action from the given data.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryAction">The try action.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool Try<TType>([NotNull] this TType @this, Action<TType> tryAction)
        {
            try
            {
                tryAction(@this);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>A TType extension method that attempts to action from the given data.</summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="tryAction">The try action.</param>
        /// <param name="catchAction">The catch action.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool Try<TType>([NotNull] this TType @this, Action<TType> tryAction, Action<TType> catchAction)
        {
            try
            {
                tryAction(@this);
                return true;
            }
            catch
            {
                catchAction(@this);
                return false;
            }
        }

        /// <summary>
        ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
        public static bool InRange<T>([NotNull] this T @this, T minValue, T maxValue) where T : IComparable<T>
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        /// <summary>
        ///     A T extension method that query if 'source' is the default value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <returns>true if default, false if not.</returns>
        public static bool IsDefault<T>(this T source)
        {
            return typeof(T).IsValueType ? source.Equals(default(T)) : source == null;
        }

        /// <summary>
        ///     An object extension method that converts the @this to string or return an empty string if the value is null.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a string or empty if the value is null.</returns>
        public static string ToSafeString(this object @this)
        {
            return @this == null ? string.Empty : @this.ToString();
        }

        #endregion object

        #region object[]

        /// <summary>
        ///     Gets the types of the objects in the specified array.
        /// </summary>
        /// <param name="args">An array of objects whose types to determine.</param>
        /// <returns>An array of  objects representing the types of the corresponding elements in .</returns>
        public static Type[] GetTypeArray([NotNull] this object[] args)
        {
            return Type.GetTypeArray(args);
        }

        #endregion object[]

        #region Random

        /// <summary>
        ///     A Random extension method that return a random value from the specified values.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing arguments.</param>
        /// <returns>One of the specified value.</returns>
        public static T OneOf<T>([NotNull] this Random @this, params T[] values)
        {
            return values[@this.Next(values.Length)];
        }

        /// <summary>
        ///     A Random extension method that flip a coin toss.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true 50% of time, otherwise false.</returns>
        public static bool CoinToss([NotNull] this Random @this)
        {
            return @this.Next(2) == 0;
        }

        #endregion Random

        #region string

        /// <summary>
        ///     A T extension method that query if '@this' is null.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null, false if not.</returns>
        public static bool IsNull(this string @this)
        {
            return @this == null;
        }

        /// <summary>
        ///     A string extension method that query if '@this' is null or empty.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null or empty, false if not.</returns>
        public static bool IsNullOrEmpty(this string @this) => string.IsNullOrEmpty(@this);

        /// <summary>
        ///     A string extension method that query if '@this' is not null and not empty.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>false if null or empty, true if not.</returns>
        public static bool IsNotNullOrEmpty(this string @this)
            => !string.IsNullOrEmpty(@this);

        /// <summary>
        ///     A string extension method that query if '@this' is null or whiteSpace.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if null or whiteSpace, false if not.</returns>
        public static bool IsNullOrWhiteSpace(this string @this) => string.IsNullOrWhiteSpace(@this);

        /// <summary>
        ///     A string extension method that query if '@this' is not null and not whiteSpace.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>false if null or whiteSpace, true if not.</returns>
        public static bool IsNotNullOrWhiteSpace(this string @this) => !string.IsNullOrWhiteSpace(@this);

        /// <summary>
        ///     Creates a new instance of  with the same value as a specified .
        /// </summary>
        /// <param name="str">The string to copy.</param>
        /// <returns>A new string with the same value as .</returns>
        public static string Copy([NotNull] this string str) => string.Copy(str);

        /// <summary>
        ///     Retrieves the system&#39;s reference to the specified .
        /// </summary>
        /// <param name="str">A string to search for in the intern pool.</param>
        /// <returns>
        ///     The system&#39;s reference to , if it is interned; otherwise, a new reference to a string with the value of .
        /// </returns>
        public static string Intern([NotNull] this string str) => string.Intern(str);

        /// <summary>
        ///     Retrieves a reference to a specified .
        /// </summary>
        /// <param name="str">The string to search for in the intern pool.</param>
        /// <returns>A reference to  if it is in the common language runtime intern pool; otherwise, null.</returns>
        public static string IsInterned([NotNull] this string str) => string.IsInterned(str);

        /// <summary>
        ///     Concatenates the elements of an object array, using the specified separator between each element.
        /// </summary>
        /// <param name="separator">
        ///     The string to use as a separator.  is included in the returned string only if  has more
        ///     than one element.
        /// </param>
        /// <param name="values">An array that contains the elements to concatenate.</param>
        /// <returns>
        ///     A string that consists of the elements of  delimited by the  string. If  is an empty array, the method
        ///     returns .
        /// </returns>
        public static string Join<T>([NotNull] this string separator, IEnumerable<T> values) => string.Join(separator, values);

        /// <summary>
        ///     Indicates whether the specified regular expression finds a match in the specified input string.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <returns>true if the regular expression finds a match; otherwise, false.</returns>
        public static bool IsMatch([NotNull] this string input, string pattern) => Regex.IsMatch(input, pattern);

        /// <summary>
        ///     Indicates whether the specified regular expression finds a match in the specified input string, using the
        ///     specified matching options.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="options">A bitwise combination of the enumeration values that provide options for matching.</param>
        /// <returns>true if the regular expression finds a match; otherwise, false.</returns>
        public static bool IsMatch([NotNull] this string input, string pattern, RegexOptions options) => Regex.IsMatch(input, pattern, options);

        /// <summary>An IEnumerable&lt;string&gt; extension method that concatenates the given this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A string.</returns>
        public static string Concatenate([NotNull] this IEnumerable<string> @this)
        {
            var sb = new StringBuilder();

            foreach (var s in @this)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }

        /// <summary>An IEnumerable&lt;T&gt; extension method that concatenates.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">The source to act on.</param>
        /// <param name="func">The function.</param>
        /// <returns>A string.</returns>
        public static string Concatenate<T>([NotNull] this IEnumerable<T> source, Func<T, string> func)
        {
            var sb = new StringBuilder();
            foreach (var item in source)
            {
                sb.Append(func(item));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     A string extension method that query if this object contains the given value.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the value is in the string, false if not.</returns>
        public static bool Contains([NotNull] this string @this, string value) => @this.IndexOf(value, StringComparison.Ordinal) != -1;

        /// <summary>
        ///     A string extension method that query if this object contains the given value.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>true if the value is in the string, false if not.</returns>
        public static bool Contains([NotNull] this string @this, string value, StringComparison comparisonType) => @this.IndexOf(value, comparisonType) != -1;

        /// <summary>
        ///     A string extension method that extracts this object.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A string.</returns>
        public static string Extract([NotNull] this string @this, Func<char, bool> predicate) => new string(@this.ToCharArray().Where(predicate).ToArray());

        /// <summary>
        ///     A string extension method that removes the letter.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A string.</returns>
        public static string RemoveWhere([NotNull] this string @this, Func<char, bool> predicate) => new string(@this.ToCharArray().Where(x => !predicate(x)).ToArray());

        /// <summary>
        ///     Replaces the format item in a specified String with the text equivalent of the value of a corresponding
        ///     Object instance in a specified array.
        /// </summary>
        /// <param name="this">A String containing zero or more format items.</param>
        /// <param name="values">An Object array containing zero or more objects to format.</param>
        /// <returns>
        ///     A copy of format in which the format items have been replaced by the String equivalent of the corresponding
        ///     instances of Object in args.
        /// </returns>
        public static string FormatWith([NotNull] this string @this, params object[] values) => string.Format(@this, values);

        /// <summary>
        ///     A string extension method that query if '@this' satisfy the specified pattern.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="pattern">The pattern to use. Use '*' as wildcard string.</param>
        /// <returns>true if '@this' satisfy the specified pattern, false if not.</returns>
        public static bool IsLike([NotNull] this string @this, string pattern)
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

            return Regex.IsMatch(@this, regexPattern);
        }

        /// <summary>
        /// SafeSubstring
        /// </summary>
        /// <param name="this"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string SafeSubstring([NotNull] this string @this, int startIndex)
        {
            if (startIndex < 0 || startIndex > @this.Length)
            {
                return string.Empty;
            }
            return @this.Substring(startIndex);
        }

        /// <summary>
        /// SafeSubstring
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SafeSubstring([NotNull] this string str, int startIndex, int length)
        {
            if (startIndex < 0 || startIndex >= str.Length || length < 0)
            {
                return string.Empty;
            }
            return str.Substring(startIndex, Math.Min(str.Length - startIndex, length));
        }

        /// <summary>
        /// Sub, not only substring but support for negative numbers
        /// </summary>
        /// <param name="this">string to be handled</param>
        /// <param name="startIndex">startIndex to substract</param>
        /// <returns>substring</returns>
        public static string Sub([NotNull] this string @this, int startIndex)
        {
            if (startIndex >= 0)
            {
                return @this.SafeSubstring(startIndex);
            }
            if (Math.Abs(startIndex) > @this.Length)
            {
                return string.Empty;
            }
            return @this.Substring(@this.Length + startIndex);
        }

        /// <summary>
        ///     A string extension method that repeats the string a specified number of times.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="repeatCount">Number of repeats.</param>
        /// <returns>The repeated string.</returns>
        public static string Repeat([NotNull] this string @this, int repeatCount)
        {
            if (@this.Length == 1)
            {
                return new string(@this[0], repeatCount);
            }

            var sb = new StringBuilder(repeatCount * @this.Length);
            while (repeatCount-- > 0)
            {
                sb.Append(@this);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     A string extension method that reverses the given string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The string reversed.</returns>
        public static string Reverse([NotNull] this string @this)
        {
            if (@this.Length <= 1)
            {
                return @this;
            }

            var chars = @this.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        ///     Returns a String array containing the substrings in this string that are delimited by elements of a specified
        ///     String array. A parameter specifies whether to return empty array elements.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="separator">A string that delimit the substrings in this string.</param>
        /// <param name="option">
        ///     (Optional) Specify RemoveEmptyEntries to omit empty array elements from the array returned,
        ///     or None to include empty array elements in the array returned.
        /// </param>
        /// <returns>
        ///     An array whose elements contain the substrings in this string that are delimited by the separator.
        /// </returns>
        public static string[] Split([NotNull] this string @this, string separator, StringSplitOptions option = StringSplitOptions.None) => @this.Split(new[] { separator }, option);

        /// <summary>
        ///     A string extension method that converts the @this to a byte array.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a byte[].</returns>
        public static byte[] ToByteArray([NotNull] this string @this) => Encoding.UTF8.GetBytes(@this);

        /// <summary>
        ///     A string extension method that converts the @this to a byte array.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">encoding</param>
        /// <returns>@this as a byte[].</returns>
        public static byte[] ToByteArray([NotNull] this string @this, Encoding encoding) => encoding.GetBytes(@this);

        public static byte[] GetBytes([NotNull] this string str) => str.GetBytes(Encoding.UTF8);

        public static byte[] GetBytes([NotNull] this string str, Encoding encoding) => encoding.GetBytes(str);

        /// <summary>
        ///     A string extension method that converts the @this to an enum.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a T.</returns>
        public static T ToEnum<T>([NotNull] this string @this) => (T)Enum.Parse(typeof(T), @this);

        /// <summary>
        ///     A string extension method that converts the @this to a title case.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a string.</returns>
        public static string ToTitleCase([NotNull] this string @this) => new CultureInfo("en-US").TextInfo.ToTitleCase(@this);

        /// <summary>
        ///     A string extension method that converts the @this to a title case.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="cultureInfo">Information describing the culture.</param>
        /// <returns>@this as a string.</returns>
        public static string ToTitleCase([NotNull] this string @this, CultureInfo cultureInfo) => cultureInfo.TextInfo.ToTitleCase(@this);

        /// <summary>
        ///     A string extension method that truncates.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>A string.</returns>
        public static string Truncate(this string @this, int maxLength) => @this.Truncate(maxLength, "...");

        /// <summary>
        ///     A string extension method that truncates.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>A string.</returns>
        public static string Truncate(this string @this, int maxLength, string suffix)
        {
            if (@this == null || @this.Length <= maxLength)
            {
                return @this;
            }
            return @this.Substring(0, maxLength - suffix.Length) + suffix;
        }

        /// <summary>
        /// EqualsIgnoreCase
        /// </summary>
        /// <param name="s1">string1</param>
        /// <param name="s2">string2</param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string s1, string s2)
            => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);

        #endregion string

        #region StringBuilder

        /// <summary>A StringBuilder extension method that substrings.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>A string.</returns>
        public static string Substring([NotNull] this StringBuilder @this, int startIndex)
        {
            return @this.ToString(startIndex, @this.Length - startIndex);
        }

        /// <summary>A StringBuilder extension method that substrings.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>A string.</returns>
        public static string Substring([NotNull] this StringBuilder @this, int startIndex, int length)
        {
            return @this.ToString(startIndex, length);
        }

        /// <summary>A StringBuilder extension method that appends a join.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="values">The values.</param>
        public static StringBuilder AppendJoin<T>([NotNull] this StringBuilder @this, string separator, IEnumerable<T> values)
        {
            @this.Append(string.Join(separator, values));

            return @this;
        }

        /// <summary>A StringBuilder extension method that appends a line join.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="values">The values.</param>
        public static StringBuilder AppendLineJoin<T>([NotNull] this StringBuilder @this, string separator, IEnumerable<T> values)
        {
            @this.AppendLine(string.Join(separator, values));

            return @this;
        }

        #endregion StringBuilder

        #region TimeSpan

        /// <summary>
        ///     A TimeSpan extension method that substract the specified TimeSpan to the current DateTime.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The current DateTime with the specified TimeSpan substracted from it.</returns>
        public static DateTime Ago(this TimeSpan @this) => DateTime.Now.Subtract(@this);

        /// <summary>
        ///     A TimeSpan extension method that add the specified TimeSpan to the current DateTime.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The current DateTime with the specified TimeSpan added to it.</returns>
        public static DateTime FromNow(this TimeSpan @this) => DateTime.Now.Add(@this);

        /// <summary>
        ///     A TimeSpan extension method that substract the specified TimeSpan to the current UTC (Coordinated Universal
        ///     Time)
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The current UTC (Coordinated Universal Time) with the specified TimeSpan substracted from it.</returns>
        public static DateTime UtcAgo(this TimeSpan @this) => DateTime.UtcNow.Subtract(@this);

        /// <summary>
        ///     A TimeSpan extension method that add the specified TimeSpan to the current UTC (Coordinated Universal Time)
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The current UTC (Coordinated Universal Time) with the specified TimeSpan added to it.</returns>
        public static DateTime UtcFromNow(this TimeSpan @this) => DateTime.UtcNow.Add(@this);

        #endregion TimeSpan

        #region Type

        /// <summary>
        ///     A Type extension method that creates an instance.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The new instance.</returns>
        public static T CreateInstance<T>([NotNull] this Type @this) => (T)Activator.CreateInstance(@this);

        /// <summary>
        ///     A Type extension method that creates an instance.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The new instance.</returns>
        public static T CreateInstance<T>([NotNull] this Type @this, params object[] args) => (T)Activator.CreateInstance(@this, args);

        /// <summary>
        /// if a type has empty constructor
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static bool HasEmptyConstructor([NotNull] this Type type)
            => type.GetConstructors(BindingFlags.Instance).Any(c => c.GetParameters().Length == 0);

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static readonly ConcurrentDictionary<Type, object> _defaultValues =
            new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// 根据 Type 获取默认值，实现类似 default(T) 的功能
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static object GetDefaultValue([NotNull] this Type type) =>
            type.IsValueType && type != typeof(void) ? _defaultValues.GetOrAdd(type, Activator.CreateInstance) : null;

        /// <summary>
        /// GetUnderlyingType if nullable else return self
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static Type Unwrap([NotNull] this Type type)
            => Nullable.GetUnderlyingType(type) ?? type;

        /// <summary>
        /// GetUnderlyingType
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static Type GetUnderlyingType([NotNull] this Type type)
            => Nullable.GetUnderlyingType(type);

        #endregion Type
    }
}