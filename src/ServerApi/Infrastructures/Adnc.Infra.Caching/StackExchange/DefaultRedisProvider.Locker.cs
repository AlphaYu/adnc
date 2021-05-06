using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Adnc.Infra.Caching.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider: Adnc.Infra.Caching.IDistributedLocker
    {
        public bool Lock(string cacheKey, string cacheValue, TimeSpan? expiration = null)
        {
            if (expiration == null) expiration = TimeSpan.FromMilliseconds(_cacheOptions.LockMs);
            bool flag = _redisDb.Lock(cacheKey, cacheValue, expiration);
            return flag;
        }

        public async Task<bool> LockAsync(string cacheKey, string cacheValue, TimeSpan? expiration = null)
        {
            if (expiration == null) expiration = TimeSpan.FromMilliseconds(_cacheOptions.LockMs);
            bool flag = await _redisDb.LockAsync(cacheKey, cacheValue, expiration);
            return flag;
        }

        public bool SafedUnLock(string cacheKey, string cacheValue)
        {
            return _redisDb.SafedUnLock(cacheKey, cacheValue);
        }

        public async Task<bool> SafedUnLockAsync(string cacheKey, string cacheValue)
        {
            return  await _redisDb.SafedUnLockAsync(cacheKey, cacheValue);
        }
    }
}
