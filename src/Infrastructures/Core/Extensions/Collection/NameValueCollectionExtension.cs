namespace System.Collections.Specialized;

/// <summary>
/// Extension methods for NameValueCollection.
/// </summary>
public static class NameValueCollectionExtension
{
    /// <summary>
    /// Converts a name-value collection into a query string in the format key1=value1&amp;key2=value2. 
    /// Keys and values are URL-encoded.
    /// </summary>
    /// <param name="source">The data source.</param>
    /// <returns>The formatted query string.</returns>
    public static string ToQueryString(this NameValueCollection source)
    {
        if (source is null)
        {
            return string.Empty;
        }

        var sb = new StringBuilder(1024);

        foreach (var key in source.AllKeys)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }
            sb.Append('&');
            sb.Append(WebUtility.UrlEncode(key));
            sb.Append('=');
            var val = source.Get(key);
            if (val != null)
            {
                sb.Append(WebUtility.UrlEncode(val));
            }
        }

        return sb.Length > 0 ? sb.ToString(1, sb.Length - 1) : "";
    }
}
