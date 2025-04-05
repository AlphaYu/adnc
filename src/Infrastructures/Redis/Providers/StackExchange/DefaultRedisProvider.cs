using Adnc.Infra.Redis.Configurations;
using Adnc.Infra.Redis.Core;
using Adnc.Infra.Redis.Core.Serialization;
using StackExchange.Redis;

namespace Adnc.Infra.Redis.Providers.StackExchange;

/// <summary>
/// Default redis caching provider.
/// </summary>
public partial class DefaultRedisProvider : IRedisProvider
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger? _logger;

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
    /// Initializes a new instance of the <see cref="DefaultRedisProvider" /> class.
    /// </summary>
    /// <param name="dbProviders"></param>
    /// <param name="redisOptions"></param>
    /// <param name="serializer"></param>
    /// <param name="loggerFactory"></param>
    public DefaultRedisProvider(
        DefaultDatabaseProvider dbProviders,
        IOptions<RedisOptions> redisOptions,
        ISerializer serializer,
        ILoggerFactory? loggerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(dbProviders, nameof(dbProviders));
        ArgumentNullException.ThrowIfNull(serializer, nameof(serializer));
        _serializer = serializer;
        _logger = loggerFactory?.CreateLogger<DefaultRedisProvider>();
        _redisDb = dbProviders.GetDatabase();
        _servers = dbProviders.GetServerList();
    }

    public string Name => ConstValue.Provider.StackExchange;

    /// <summary>
    /// The serializer.
    /// </summary>
    public ISerializer Serializer => _serializer;
}
