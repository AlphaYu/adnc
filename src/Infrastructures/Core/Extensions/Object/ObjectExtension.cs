namespace System;

public static class ObjectExtension
{
    /// <summary>
    /// Remove leading and trailing spaces from all string fields.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    public static void TrimStringFields<T>(this T obj) where T : class, new()
    {
        if (obj is null)
        {
            return;
        }

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
