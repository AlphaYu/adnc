using Adnc.Infra.Repository.Mongo.Configuration;
using Humanizer;

namespace Adnc.Infra.Repository.Mongo.Extensions
{
    public static class MongoConfigurationExtensions
    {
        /// <summary>
        /// Gets the name of the mongo collection configured for the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="options">The configuration.</param>
        /// <returns></returns>
        public static string GetCollectionName<TEntity>(this MongoRepositoryOptions options)
        {
            var name = typeof(TEntity).Name;
            if (options.PluralizeCollectionNames)
            {
                name = name.Pluralize();
            }

            switch (options.CollectionNamingConvention)
            {
                case NamingConvention.LowerCase:
                    return name.ToLower();

                case NamingConvention.UpperCase:
                    return name.ToUpper();

                case NamingConvention.Pascal:
                    return name.Pascalize();

                case NamingConvention.Camel:
                    return name.Camelize();

                case NamingConvention.Snake:
                    return name.Underscore();

                default:
                    throw new ArgumentOutOfRangeException(nameof(options.CollectionNamingConvention),
                                                          options.CollectionNamingConvention,
                                                          "Unknown collection naming convention");
            }
        }
    }
}