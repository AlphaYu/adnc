namespace System;

/// <summary>
/// Provides extension methods for working with System.
/// </summary>
public static class ByteArrayExtension
{
    /// <summary>
    /// Converts the byte array to a MemoryStream.
    /// </summary>
    /// <param name="byteArray">The byte array to convert.</param>
    /// <returns>The MemoryStream.</returns>
    public static MemoryStream ToMemoryStream(this byte[] byteArray)
    {
        if (byteArray == null)
        {
            throw new ArgumentNullException(nameof(byteArray));
        }

        return new MemoryStream(byteArray);
    }

    /// <summary>
    /// Converts the byte array to a hex string.
    /// </summary>
    /// <param name="bytes">The byte array to convert.</param>
    /// <returns>The hex string.</returns>
    public static string ToHexString(this byte[] bytes)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("X2"));
        }
        return sb.ToString();
    }
}