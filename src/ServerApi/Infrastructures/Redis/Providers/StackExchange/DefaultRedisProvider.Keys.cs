using Adnc.Infra.Redis.Core;

namespace Adnc.Infra.Redis.Providers.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : IRedisProvider
    {
        public bool KeyDel(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var flag = _redisDb.KeyDelete(cacheKey);
            return flag;
        }

        public async Task<bool> KeyDelAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var flag = await _redisDb.KeyDeleteAsync(cacheKey);
            return flag;
        }

        public bool KeyExpire(string cacheKey, int second)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var flag = _redisDb.KeyExpire(cacheKey, TimeSpan.FromSeconds(second));
            return flag;
        }

        public async Task<bool> KeyExpireAsync(string cacheKey, int second)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var flag = await _redisDb.KeyExpireAsync(cacheKey, TimeSpan.FromSeconds(second));
            return flag;
        }

        public bool KeyExists(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var flag = _redisDb.KeyExists(cacheKey);
            return flag;
        }

        public async Task<bool> KeyExistsAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var flag = await _redisDb.KeyExistsAsync(cacheKey);
            return flag;
        }

        public long TTL(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var ts = _redisDb.KeyTimeToLive(cacheKey);
            return ts.HasValue ? (long)ts.Value.TotalSeconds : -1;
        }

        public async Task<long> TTLAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var ts = await _redisDb.KeyTimeToLiveAsync(cacheKey);
            return ts.HasValue ? (long)ts.Value.TotalSeconds : -1;
        }
    }
}