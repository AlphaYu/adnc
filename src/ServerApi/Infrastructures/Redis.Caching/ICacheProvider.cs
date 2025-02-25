using Adnc.Infra.Redis.Caching.Configurations;
using Adnc.Infra.Redis.Caching.Core;
using Adnc.Infra.Redis.Core.Serialization;

namespace Adnc.Infra.Redis.Caching;

public interface ICacheProvider
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }

    IOptions<CacheOptions> CacheOptions { get; }

    /// <summary>
    /// The serializer.
    /// </summary>
    ISerializer Serializer { get; }

    /// <summary>
    /// Set the specified cacheKey, cacheValue and expiration.
    /// </summary>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    void Set<T>(string cacheKey, T cacheValue, TimeSpan expiration);

    /// <summary>
    /// Sets the specified cacheKey, cacheValue and expiration async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration);

    /// <summary>
    /// Get the specified cacheKey.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    CacheValue<T> Get<T>(string cacheKey);

    /// <summary>
    /// Get the specified cacheKey async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task<CacheValue<T>> GetAsync<T>(string cacheKey);

    /// <summary>
    /// Remove the specified cacheKey.
    /// </summary>
    /// <param name="cacheKey">Cache key.</param>
    void Remove(string cacheKey);

    /// <summary>
    /// Remove the specified cacheKey async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    Task RemoveAsync(string cacheKey);

    /// <summary>
    /// Exists the specified cacheKey async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    Task<bool> ExistsAsync(string cacheKey);

    /// <summary>
    /// Exists the specified cacheKey.
    /// </summary>
    /// <returns>The exists.</returns>
    /// <param name="cacheKey">Cache key.</param>
    bool Exists(string cacheKey);

    /// <summary>
    /// Tries the set.
    /// </summary>
    /// <returns><c>true</c>, if set was tryed, <c>false</c> otherwise.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    bool TrySet<T>(string cacheKey, T cacheValue, TimeSpan expiration);

    /// <summary>
    /// Tries the set async.
    /// </summary>
    /// <returns>The set async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="cacheValue">Cache value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task<bool> TrySetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration);

    /// <summary>
    /// Sets all.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    void SetAll<T>(IDictionary<string, T> value, TimeSpan expiration);

    /// <summary>
    /// Sets all async.
    /// </summary>
    /// <returns>The all async.</returns>
    /// <param name="value">Value.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task SetAllAsync<T>(IDictionary<string, T> value, TimeSpan expiration);

    /// <summary>
    /// Removes all.
    /// </summary>
    /// <param name="cacheKeys">Cache keys.</param>
    void RemoveAll(IEnumerable<string> cacheKeys);

    /// <summary>
    /// Removes all async.
    /// </summary>
    /// <returns>The all async.</returns>
    /// <param name="cacheKeys">Cache keys.</param>
    Task RemoveAllAsync(IEnumerable<string> cacheKeys);

    /// <summary>
    /// Get the specified cacheKey, dataRetriever and expiration.
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="dataRetriever">Data retriever.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    CacheValue<T> Get<T>(string cacheKey, Func<T> dataRetriever, TimeSpan expiration);

    /// <summary>
    /// Gets the specified cacheKey, dataRetriever and expiration async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="dataRetriever">Data retriever.</param>
    /// <param name="expiration">Expiration.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    Task<CacheValue<T>> GetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan expiration);

    /// <summary>
    /// Removes cached item by cachekey's prefix.
    /// </summary>
    /// <param name="prefix">Prefix of CacheKey.</param>
    void RemoveByPrefix(string prefix);

    /// <summary>
    /// Removes cached item by cachekey's prefix async.
    /// </summary>
    /// <param name="prefix">Prefix of CacheKey.</param>
    Task RemoveByPrefixAsync(string prefix);

    /// <summary>
    /// Gets the specified cacheKey async.
    /// </summary>
    /// <returns>The async.</returns>
    /// <param name="cacheKey">Cache key.</param>
    /// <param name="type">Object Type.</param>
    Task<object> GetAsync(string cacheKey, Type type);

    /// <summary>
    /// Set the keys  TTL
    /// </summary>
    /// <param name="cacheKeys">Cache keys.</param>
    /// <param name="seconds">Expiration .</param>
    /// <returns></returns>
    Task KeyExpireAsync(IEnumerable<string> cacheKeys, int seconds);
}