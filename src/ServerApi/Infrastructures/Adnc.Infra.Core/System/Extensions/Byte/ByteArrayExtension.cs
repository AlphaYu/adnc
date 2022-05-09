namespace System;

public static class ByteArrayExtension
{
    /// <summary>
    ///     A byte[] extension method that converts the @this byteArray to a memory stream.
    /// </summary>
    /// <param name="byteArray">The byetArray to act on</param>
    /// <returns>@this as a MemoryStream.</returns>
    public static MemoryStream ToMemoryStream([NotNull] this byte[] byteArray) => new(byteArray);
}