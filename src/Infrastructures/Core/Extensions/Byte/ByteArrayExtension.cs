namespace System;

/// <summary>
/// /// Extension methods for ByteArrayExtension.
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
        ArgumentNullException.ThrowIfNull(byteArray, nameof(byteArray));
        return new MemoryStream(byteArray);
    }

    /// <summary>
    /// Converts the byte array to a hex string.
    /// </summary>
    /// <param name="bytes">The byte array to convert.</param>
    /// <returns>The hex string.</returns>
    public static string ToHexString(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        var sb = new StringBuilder();
        for (var i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("X2"));
        }
        return sb.ToString();
    }
}
