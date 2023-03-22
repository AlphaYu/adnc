namespace System;

public static class ObjectExtension
{
    /// <summary>
    ///  Converts the object to string or return an empty string if the value is null.
    /// </summary>
    /// <param name="obj">The @this to act on.</param>
    /// <returns>@this as a string or empty if the value is null.</returns>
    public static string ToSafeString(this object? obj)
    {
        if (obj is null)
            return string.Empty;

        return obj.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Remove leading and trailing spaces from all string fields.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    public static void TrimStringFields<T>(this T obj) where T : class
    {
        if (obj is null) return;
        var stringProperties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                        .Where(p => p.PropertyType == typeof(string));

        foreach (var property in stringProperties)
        {
            var propertyValue = (string?)property.GetValue(obj);
            if (propertyValue != null)
            {
                var trimmedValue = propertyValue.Trim();
                property.SetValue(obj, trimmedValue);
            }
        }
    }
}