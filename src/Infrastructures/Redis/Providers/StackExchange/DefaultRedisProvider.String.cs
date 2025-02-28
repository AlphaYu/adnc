using Adnc.Infra.Redis.Core;
using StackExchange.Redis;

namespace Adnc.Infra.Redis.Providers.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : Adnc.Infra.Redis.IRedisProvider
    {
        public long IncrBy(string cacheKey, long value = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = _redisDb.StringIncrement(cacheKey, value);
            return res;
        }

        public async Task<long> IncrByAsync(string cacheKey, long value = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = await _redisDb.StringIncrementAsync(cacheKey, value);
            return res;
        }

        public double IncrByFloat(string cacheKey, double value = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = _redisDb.StringIncrement(cacheKey, value);
            return res;
        }

        public async Task<double> IncrByFloatAsync(string cacheKey, double value = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = await _redisDb.StringIncrementAsync(cacheKey, value);
            return res;
        }

        public bool StringSet(string cacheKey, string cacheValue, System.TimeSpan? expiration = null, string when = "")
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            When w = When.Always;

            if (when.Equals("nx", StringComparison.OrdinalIgnoreCase))
            {
                w = When.NotExists;
            }
            else if (when.Equals("xx", StringComparison.OrdinalIgnoreCase))
            {
                w = When.Exists;
            }

            bool flag = _redisDb.StringSet(cacheKey, cacheValue, expiration, w);
            return flag;
        }

        public async Task<bool> StringSetAsync(string cacheKey, string cacheValue, System.TimeSpan? expiration = null, string when = "")
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            When w = When.Always;

            if (when.Equals("nx", StringComparison.OrdinalIgnoreCase))
            {
                w = When.NotExists;
            }
            else if (when.Equals("xx", StringComparison.OrdinalIgnoreCase))
            {
                w = When.Exists;
            }

            bool flag = await _redisDb.StringSetAsync(cacheKey, cacheValue, expiration, w);
            return flag;
        }

        public string StringGet(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var val = _redisDb.StringGet(cacheKey);
            return val;
        }

        public async Task<string> StringGetAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var val = await _redisDb.StringGetAsync(cacheKey);
            return val;
        }

        public long StringLen(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = _redisDb.StringLength(cacheKey);
            return len;
        }

        public async Task<long> StringLenAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = await _redisDb.StringLengthAsync(cacheKey);
            return len;
        }

        public long StringSetRange(string cacheKey, long offest, string value)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = _redisDb.StringSetRange(cacheKey, offest, value);
            return (long)res;
        }

        public async Task<long> StringSetRangeAsync(string cacheKey, long offest, string value)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = await _redisDb.StringSetRangeAsync(cacheKey, offest, value);
            return (long)res;
        }

        public string StringGetRange(string cacheKey, long start, long end)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = _redisDb.StringGetRange(cacheKey, start, end);
            return res;
        }

        public async Task<string> StringGetRangeAsync(string cacheKey, long start, long end)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var res = await _redisDb.StringGetRangeAsync(cacheKey, start, end);
            return res;
        }
    }
}