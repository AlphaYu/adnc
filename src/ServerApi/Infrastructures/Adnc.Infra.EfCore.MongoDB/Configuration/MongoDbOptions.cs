namespace Adnc.Infra.Repository.EfCore.MongoDB.Configuration;

/// <summary>
/// Mongodb配置
/// </summary>
public class MongoDbOptions
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