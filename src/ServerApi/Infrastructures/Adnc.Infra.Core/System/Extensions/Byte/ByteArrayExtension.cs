namespace System;

public static class ByteArrayExtension
{
    /// <summary>
    ///     A byte[] extension method that converts the @this byteArray to a memory stream.
    /// </summary>
    /// <param name="byteArray">The byetArray to act on</param>
    /// <returns>@this as a MemoryStream.</returns>
    public static MemoryStream ToMemoryStream([NotNull] this byte[] byteArray) => new(byteArray);

    /// <summary>
    /// byte to hex string extension
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
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