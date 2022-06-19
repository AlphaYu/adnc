using MongoDB.Driver;

namespace Adnc.Infra.Repository.Mongo.Extensions
{
    /// <summary>
    /// Extensions for options classes in MongoDB. E.g. <see cref="FindOptions{TDocument}"/>, <see cref="CreateIndexOptions{TDocument}"/>.
    /// </summary>
    public static class MongoOptionsExtensions
    {
        /// <summary>
        /// Adds a case-insensitive collation option to the specified find options.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static FindOptions<TEntity> WithCaseInsensitiveCollation<TEntity>(this FindOptions<TEntity> options)
        {
            options.Collation = CaseInsensitiveCollation;
            return options;
        }

        /// <summary>
        /// Adds a case-insensitive collation option to the specified create index options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static CreateIndexOptions WithCaseInsensitiveCollation(this CreateIndexOptions options)
        {
            options.Collation = CaseInsensitiveCollation;
            return options;
        }

        /// <summary>
        /// Turns on the unique flag in the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static CreateIndexOptions Unique(this CreateIndexOptions options)
        {
            options.Unique = true;
            return options;
        }

        /// <summary>
        /// Configures the expires after configuration in the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="expireAfter">The expire after.</param>
        /// <returns></returns>
        public static CreateIndexOptions ExpireAfter(this CreateIndexOptions options, TimeSpan expireAfter)
        {
            options.ExpireAfter = expireAfter;
            return options;
        }

        private static Collation CaseInsensitiveCollation => new Collation("en", strength: CollationStrength.Secondary);
    }
}