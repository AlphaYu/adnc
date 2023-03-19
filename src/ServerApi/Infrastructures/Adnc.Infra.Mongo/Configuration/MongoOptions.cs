namespace Adnc.Infra.Core.Configuration;

/// <summary>
/// Mongodb配置
/// </summary>
public class MongoOptions
{
    /// <summary>
    /// Gets or sets the MongoDB connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection naming convention.
    /// Defaults to NamingConvention.Snake/>.
    /// </summary>
    public int CollectionNamingConvention { get; set; } = (int)MongoNamingConvention.Snake;

    /// <summary>
    /// Gets or sets a value indicating whether to pluralize collection names.
    /// Defaults to <c>true</c>.
    /// </summary>
    public bool PluralizeCollectionNames { get; set; } = true;
}

public enum MongoNamingConvention
{
    /// <summary>
    /// Convert names to "lowercase" without word separators.
    /// </summary>
    LowerCase = 0,

    /// <summary>
    /// Convert names to "UPPERCASE" without word separators.
    /// </summary>
    UpperCase = 1,

    /// <summary>
    /// Convert names to "UpperCamelCase".
    /// </summary>
    Pascal = 2,

    /// <summary>
    /// Convert names to "camelCase".
    /// </summary>
    Camel = 3,

    /// <summary>
    /// Convert names to "snake_case".
    /// </summary>
    Snake = 4
}