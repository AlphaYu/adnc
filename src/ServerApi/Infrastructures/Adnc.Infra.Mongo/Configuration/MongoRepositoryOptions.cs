namespace Adnc.Infra.Repository.Mongo.Configuration
{
    public class MongoRepositoryOptions
    {
        /// <summary>
        /// Gets or sets the MongoDB connection string.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection naming convention.
        /// Defaults to <see cref="NamingConvention.Snake"/>.
        /// </summary>
        public NamingConvention CollectionNamingConvention { get; set; } = NamingConvention.Snake;

        /// <summary>
        /// Gets or sets a value indicating whether to pluralize collection names.
        /// Defaults to <c>true</c>.
        /// </summary>
        public bool PluralizeCollectionNames { get; set; } = true;
    }
}