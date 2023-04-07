using Adnc.Infra.Redis.Caching.Configurations;
using Adnc.Infra.Redis.Caching.Core;
using Adnc.Infra.Redis.Configurations;
using Adnc.Infra.Redis.Core;
using Adnc.Infra.Redis.Core.Serialization;
using Adnc.Infra.Redis.Providers.StackExchange;

namespace Adnc.Infra.Redis.Caching.Provider;

/// <summary>
/// Default redis caching provider.
/// </summary>
public partial class DefaultCachingProvider : AbstracCacheProvider, ICacheProvider
{
    /// <summary>
    /// The cache.
    /// </summary>
    private readonly IDatabase _redisDb;

    /// <summary>
    /// The servers.
    /// </summary>
    private readonly IEnumerable<IServer> _servers;

    /// <summary>
    /// The db provider.
    /// </summary>
    private readonly DefaultDatabaseProvider _dbProvider;

    /// <summary>
    /// The serializer.
    /// </summary>
    private readonly ISerializer _serializer;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger? _logger;

    /// <summary>
    /// The redis options.
    /// </summary>
    private readonly IOptions<RedisOptions> _redisOptions;

    /// <summary>
    /// The cache options.
    /// </summary>
    private readonly IOptions<CacheOptions> _cacheOptions;

    public override IOptions<CacheOptions> CacheOptions => _cacheOptions;

    /// <summary>
    /// The cache stats.
    /// </summary>
    private readonly CacheStats _cacheStats;

    public override string Name => CachingConstValue.Provider.StackExchange;

    public override string CachingProviderType => Name + "." + nameof(DefaultCachingProvider);

    /// <summary>
    /// The serializer.
    /// </summary>
    public override ISerializer Serializer => _serializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Adnc.Infra.Redis.Redis.DefaultRedisCachingProvider"/> class.
    /// </summary>
    /// <param name="dbProviders">Db providers.</param>
    /// <param name="serializer">serializer.</param>
    /// <param name="cacheOptions">CacheOptions.</param>
    /// <param name="loggerFactory">Logger factory.</param>
    public DefaultCachingProvider(
        DefaultDatabaseProvider dbProviders,
        ISerializer serializer,
        IOptions<RedisOptions> redisOptions,
        IOptions<CacheOptions> cacheOptions,
        ILoggerFactory? loggerFactory = null)
    {
        ArgumentCheck.NotNull(dbProviders, nameof(dbProviders));

        _dbProvider = dbProviders;
        _serializer = serializer;
        _redisOptions = redisOptions;
        _cacheOptions = cacheOptions;
        _logger = loggerFactory?.CreateLogger<DefaultCachingProvider>();
        _redisDb = _dbProvider.GetDatabase();
        _servers = _dbProvider.GetServerList();
        _cacheStats = new CacheStats();
    }

    /// <summary>
    /// Get the specified cacheKey, dataRetriever and expiration.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="dataRetriever">Data retriever.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    protected override CacheValue<T> BaseGet<T>(string cacheKey, Func<T> dataRetriever, TimeSpan expiration)
    {
        ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
        ArgumentCheck.NotNegativeOrZero(expiration, nameof(expiration));

        if (!_cacheOptions.Value.PenetrationSetting.Disable && _redisOptions.Value.EnableBloomFilter)
        {
            var exists = _redisDb.BfExistsAsync(_cacheOptions.Value.PenetrationSetting.BloomFilterSetting.Name, cacheKey).GetAwaiter().GetResult();
            if (!exists)
            {
                if (_cacheOptions.Value.EnableLogging)
                    _logger?.LogInformation($"Cache Penetrated : cachekey = {cacheKey}");
                return CacheValue<T>.NoValue;
            }
        }

        var result = _redisDb.StringGet(cacheKey);
        if (!result.IsNull)
        {
            _cacheStats.OnHit();

            if (_cacheOptions.Value.EnableLogging)
                _logger?.LogInformation($"Cache Hit : cachekey = {cacheKey}");

            var value = _serializer.Deserialize<T>(result);
            return new CacheValue<T>(value, true);
        }

        _cacheStats.OnMiss();

        if (_cacheOptions.Value.EnableLogging)
            _logger?.LogInformation($"Cache Missed : cachekey = {cacheKey}");

        var flag = _redisDb.Lock(cacheKey, _cacheOptions.Value.LockMs / 1000);
        if (!flag.Success)
        {
            Thread.Sleep(_cacheOptions.Value.SleepMs);
            return Get(cacheKey, dataRetriever, expiration);
        }

        try
        {
            var item = dataRetriever();
            if (item != null)
            {
                Set(cacheKey, item, expiration);
                return new CacheValue<T>(item, true);
            }
            else
            {
                return CacheValue<T>.NoValue;
            }
        }
        catch (Exception ex)
        {
            throw new RedisException(ex.Message, ex);
        }
        finally
        {
            //remove mutex key
            _redisDb.SafedUnLock(cacheKey, flag.LockValue);
        }
    }

    /// <summary>
    /// Get the specified cacheKey.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    protected override CacheValue<T> BaseGet<T>(string cacheKey)
    {
        ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

        if (!_cacheOptions.Value.PenetrationSetting.Disable && _redisOptions.Value.EnableBloomFilter)
        {
            var exists = _redisDb.BfExistsAsync(_cacheOptions.Value.PenetrationSetting.BloomFilterSetting.Name, cacheKey).GetAwaiter().GetResult();
            if (!exists)
            {
                if (_cacheOptions.Value.EnableLogging)
                    _logger?.LogInformation($"Cache Penetrated : cachekey = {cacheKey}");
                return CacheValue<T>.NoValue;
            }
        }

        var result = _redisDb.StringGet(cacheKey);
        if (!result.IsNull)
        {
            _cacheStats.OnHit();

            if (_cacheOptions.Value.EnableLogging)
                _logger?.LogInformation($"Cache Hit : cachekey = {cacheKey}");

            var value = _serializer.Deserialize<T>(result);
            return new CacheValue<T>(value, true);
        }
        else
        {
            _cacheStats.OnMiss();

            if (_cacheOptions.Value.EnableLogging)
                _logger?.LogInformation($"Cache Missed : cachekey = {cacheKey}");

            return CacheValue<T>.NoValue;
        }
    }

    /// <summary>
    /// Remove the specified cacheKey.
    /// </summary>
    /// <returns>The remove.</returns>
    /// <param name="cacheKey">Cache key.</param>
    protected override void BaseRemove(string cacheKey)
    {
        ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

        _redisDb.KeyDelete(cacheKey);
    }

    /// <summary>
    /// Set the specified cacheKey, cacheValue and expiration.
    /// </summary>
    /// <returns>The set.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    protected override void BaseSet<T>(string cacheKey, T cacheValue, TimeSpan expiration)
    {
        ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
        ArgumentCheck.NotNull(cacheValue, nameof(cacheValue));
        ArgumentCheck.NotNegativeOrZero(expiration, nameof(expiration));

        if (_cacheOptions.Value.MaxRdSecond > 0)
        {
            var addSec = new Random().Next(1, _cacheOptions.Value.MaxRdSecond);
            expiration = expiration.Add(TimeSpan.FromSeconds(addSec));
        }

        _redisDb.StringSet(
            cacheKey,
            _serializer.Serialize(cacheValue),
            expiration);
    }

    /// <summary>
    /// Exists the specified cacheKey.
    /// </summary>
    /// <returns>The exists.</returns>
    /// <param name="cacheKey">Cache key.</param>
    protected override bool BaseExists(string cacheKey)
    {
        ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

        return _redisDb.KeyExists(cacheKey);
    }

    /// <summary>
    /// Removes cached item by cachekey's prefix.
    /// </summary>
    /// <param name="prefix">Prefix of CacheKey.</param>
    protected override void BaseRemoveByPrefix(string prefix)
    {
        ArgumentCheck.NotNullOrWhiteSpace(prefix, nameof(prefix));

        prefix = HandlePrefix(prefix);

        if (_cacheOptions.Value.EnableLogging)
            _logger?.LogInformation($"RemoveByPrefix : prefix = {prefix}");

        var redisKeys = SearchRedisKeys(prefix);

        _redisDb.KeyDelete(redisKeys);
    }

    /// <summary>
    /// Searchs the redis keys.
    /// </summary>
    /// <returns>The redis keys.</returns>
    /// <remarks>
    /// If your Redis Servers support command SCAN ,
    /// IServer.Keys will use command SCAN to find out the keys.
    /// Following
    /// https://github.com/StackExchange/StackExchange.Redis/blob/master/StackExchange.Redis/StackExchange/Redis/RedisServer.cs#L289
    /// </remarks>
    /// <param name="pattern">Pattern.</param>
    private RedisKey[] SearchRedisKeys(string pattern)
    {
        var keys = new List<RedisKey>();

        foreach (var server in _servers)
            // the default pageSize is 10, if there are too many keys here, it will hurt performance
            // see https://github.com/dotnetcore/Adnc.Infra.Redis/pull/199 for more information
            // from this redis dev specification, https://yq.aliyun.com/articles/531067 , maybe the appropriate scope is 100~500, using 200 here.
            keys.AddRange(server.Keys(pattern: pattern, database: _redisDb.Database, pageSize: 200));

        return keys.Distinct().ToArray();

        //var keys = new HashSet<RedisKey>();

        //int nextCursor = 0;
        //do
        //{
        //    RedisResult redisResult = _cache.Execute("SCAN", nextCursor.ToString(), "MATCH", pattern, "COUNT", "1000");
        //    var innerResult = (RedisResult[])redisResult;

        //    nextCursor = int.Parse((string)innerResult[0]);

        //    List<RedisKey> resultLines = ((RedisKey[])innerResult[1]).ToList();

        //    keys.UnionWith(resultLines);
        //}
        //while (nextCursor != 0);

        //return keys.ToArray();
    }

    /// <summary>
    /// Handles the prefix of CacheKey.
    /// </summary>
    /// <param name="prefix">Prefix of CacheKey.</param>
    /// <exception cref="ArgumentException"></exception>
    private string HandlePrefix(string prefix)
    {
        // Forbid
        if (prefix.Equals("*"))
            throw new ArgumentException("the prefix should not equal to *");

        // Don't start with *
        prefix = new System.Text.RegularExpressions.Regex("^\\*+").Replace(prefix, "");

        // End with *
        if (!prefix.EndsWith("*", StringComparison.OrdinalIgnoreCase))
            prefix = string.Concat(prefix, "*");

        return prefix;
    }

    /// <summary>
    /// Sets all.
    /// </summary>
    /// <param name="values">Values.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    protected override void BaseSetAll<T>(IDictionary<string, T> values, TimeSpan expiration)
    {
        ArgumentCheck.NotNegativeOrZero(expiration, nameof(expiration));
        ArgumentCheck.NotNullAndCountGTZero(values, nameof(values));

        var batch = _redisDb.CreateBatch();

        foreach (var item in values)
            batch.StringSetAsync(item.Key, _serializer.Serialize(item.Value), expiration);

        batch.Execute();
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>The all.</returns>
    /// <param name="cacheKeys">Cache keys.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    protected override IDictionary<string, CacheValue<T>> BaseGetAll<T>(IEnumerable<string> cacheKeys)
    {
        ArgumentCheck.NotNullAndCountGTZero(cacheKeys, nameof(cacheKeys));

        var keyArray = cacheKeys.ToArray();
        var values = _redisDb.StringGet(keyArray.Select(k => (RedisKey)k).ToArray());

        var result = new Dictionary<string, CacheValue<T>>();
        for (int i = 0; i < keyArray.Length; i++)
        {
            var cachedValue = values[i];
            if (!cachedValue.IsNull)
                result.Add(keyArray[i], new CacheValue<T>(_serializer.Deserialize<T>(cachedValue), true));
            else
                result.Add(keyArray[i], CacheValue<T>.NoValue);
        }

        return result;
    }

    /// <summary>
    /// Gets the by prefix.
    /// </summary>
    /// <returns>The by prefix.</returns>
    /// <param name="prefix">Prefix.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    protected override IDictionary<string, CacheValue<T>> BaseGetByPrefix<T>(string prefix)
    {
        ArgumentCheck.NotNullOrWhiteSpace(prefix, nameof(prefix));

        prefix = HandlePrefix(prefix);

        var redisKeys = SearchRedisKeys(prefix);

        var values = _redisDb.StringGet(redisKeys).ToArray();

        var result = new Dictionary<string, CacheValue<T>>();
        for (int i = 0; i < redisKeys.Length; i++)
        {
            var cachedValue = values[i];
            if (!cachedValue.IsNull)
                result.Add(redisKeys[i], new CacheValue<T>(_serializer.Deserialize<T>(cachedValue), true));
            else
                result.Add(redisKeys[i], CacheValue<T>.NoValue);
        }

        return result;
    }

    /// <summary>
    /// Removes all.
    /// </summary>
    /// <param name="cacheKeys">Cache keys.</param>
    protected override void BaseRemoveAll(IEnumerable<string> cacheKeys)
    {
        ArgumentCheck.NotNullAndCountGTZero(cacheKeys, nameof(cacheKeys));

        var redisKeys = cacheKeys.Where(k => !string.IsNullOrEmpty(k)).Select(k => (RedisKey)k).ToArray();
        if (redisKeys.Length > 0)
            _redisDb.KeyDelete(redisKeys);
    }

    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <returns>The count.</returns>
    /// <param name="prefix">Prefix.</param>
    protected override int BaseGetCount(string prefix = "")
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            var allCount = 0;

            foreach (var server in _servers)
                allCount += (int)server.DatabaseSize(_redisDb.Database);

            return allCount;
        }

        return SearchRedisKeys(HandlePrefix(prefix)).Length;
    }

    /// <summary>
    /// Flush All Cached Item.
    /// </summary>
    protected override void BaseFlush()
    {
        if (_cacheOptions.Value.EnableLogging)
            _logger?.LogInformation("Redis -- Flush");

        foreach (var server in _servers)
        {
            server.FlushDatabase(_redisDb.Database);
        }
    }

    /// <summary>
    /// Tries the set.
    /// </summary>
    /// <returns><c>true</c>, if set was tryed, <c>false</c> otherwise.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    protected override bool BaseTrySet<T>(string cacheKey, T cacheValue, TimeSpan expiration)
    {
        ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
        ArgumentCheck.NotNull(cacheValue, nameof(cacheValue));
        ArgumentCheck.NotNegativeOrZero(expiration, nameof(expiration));

        if (_cacheOptions.Value.MaxRdSecond > 0)
        {
            var addSec = new Random().Next(1, _cacheOptions.Value.MaxRdSecond);
            expiration = expiration.Add(TimeSpan.FromSeconds(addSec));
        }

        return _redisDb.StringSet(
            cacheKey,
            _serializer.Serialize(cacheValue),
            expiration,
            When.NotExists
            );
    }

    /// <summary>
    /// Get the expiration of cache key
    /// </summary>
    /// <param name="cacheKey">cache key</param>
    /// <returns>expiration</returns>
    protected override TimeSpan BaseGetExpiration(string cacheKey)
    {
        ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

        var timeSpan = _redisDb.KeyTimeToLive(cacheKey);
        return timeSpan.HasValue ? timeSpan.Value : TimeSpan.Zero;
    }
}