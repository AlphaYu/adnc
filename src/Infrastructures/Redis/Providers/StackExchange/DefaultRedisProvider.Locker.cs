using StackExchange.Redis;

namespace Adnc.Infra.Redis.Providers.StackExchange;

/// <summary>
/// Default redis caching provider.
/// </summary>
public partial class DefaultRedisProvider : Adnc.Infra.Redis.IDistributedLocker
{
    public (bool Success, string LockValue) Lock(string cacheKey, int timeoutSeconds = 5, bool autoDelay = false)
    {
        return _redisDb.Lock(cacheKey, timeoutSeconds, autoDelay);
    }

    public async Task<(bool Success, string LockValue)> LockAsync(string cacheKey, int timeoutSeconds = 5, bool autoDelay = false)
    {
        return await _redisDb.LockAsync(cacheKey, timeoutSeconds, autoDelay);
    }

    public bool SafedUnLock(string cacheKey, string cacheValue)
    {
        return _redisDb.SafedUnLock(cacheKey, cacheValue);
    }

    public async Task<bool> SafedUnLockAsync(string cacheKey, string cacheValue)
    {
        return await _redisDb.SafedUnLockAsync(cacheKey, cacheValue);
    }
}
