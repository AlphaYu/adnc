using StackExchange.Redis;

namespace Adnc.Infra.Caching.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : Adnc.Infra.Caching.IRedisProvider
    {
        public async Task<bool> BloomAddAsync(string key, string value)
        {
            return await _redisDb.BloomAddAsync(key, value);
        }

        public async Task<bool[]> BloomAddAsync(string key, IEnumerable<string> values)
        {
            var redisValues = values.Select(x => (RedisValue)x);
            return await _redisDb.BloomAddAsync(key, redisValues);
        }

        public async Task<bool> BloomExistsAsync(string key, string value)
        {
            return await _redisDb.BloomExistsAsync(key, value);
        }

        public async Task<bool[]> BloomExistsAsync(string key, IEnumerable<string> values)
        {
            var redisValues = values.Select(x => (RedisValue)x);
            return await _redisDb.BloomExistsAsync(key, redisValues);
        }

        public async Task BloomReserveAsync(string key, double errorRate, int initialCapacity)
        {
            await _redisDb.BloomReserveAsync(key, errorRate, initialCapacity);
        }
    }
}