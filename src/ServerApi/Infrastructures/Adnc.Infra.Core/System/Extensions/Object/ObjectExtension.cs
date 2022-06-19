namespace System;

public static class ObjectExtension
{
    /// <summary>
    ///     An object extension method that converts the @this to string or return an empty string if the value is null.
    /// </summary>
    /// <param name="source">The @this to act on.</param>
    /// <returns>@this as a string or empty if the value is null.</returns>
    public static string ToSafeString(this object? source)
    {
        if (source is null)
            return string.Empty;

        return source.ToString()??string.Empty;
    }
}