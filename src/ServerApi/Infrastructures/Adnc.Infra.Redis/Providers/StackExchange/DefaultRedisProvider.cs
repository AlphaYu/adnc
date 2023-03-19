using Adnc.Infra.Redis.Configurations;
using Adnc.Infra.Redis.Core;
using Adnc.Infra.Redis.Core.Serialization;
using StackExchange.Redis;

namespace Adnc.Infra.Redis.Providers.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : IRedisProvider
    {
        /// <summary>
        /// The serializer.
        /// </summary>
        public ISerializer Serializer => _serializer;

        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IDatabase _redisDb;

        /// <summary>
        /// The serializer.
        /// </summary>
        private readonly ISerializer _serializer;

        /// <summary>
        /// The servers.
        /// </summary>
        private readonly IEnumerable<IServer> _servers;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger _logger;

        public string Name => ConstValue.Provider.StackExchange;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Adnc.Infra.Redis.Redis.DefaultRedisCachingProvider"/> class.
        /// </summary>
        /// <param name="dbProviders">Db providers.</param>
        /// <param name="redisOptions">CacheOptions.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public DefaultRedisProvider(
            DefaultDatabaseProvider dbProviders,
            IOptions<RedisOptions> redisOptions,
            ISerializer serializer,
            ILoggerFactory? loggerFactory = null)
        {
            ArgumentCheck.NotNull(dbProviders, nameof(dbProviders));
            ArgumentCheck.NotNull(serializer, nameof(serializer));
            this._serializer = serializer;
            this._logger = loggerFactory?.CreateLogger<DefaultRedisProvider>();
            this._redisDb = dbProviders.GetDatabase();
            this._servers = dbProviders.GetServerList();
        }
    }
}