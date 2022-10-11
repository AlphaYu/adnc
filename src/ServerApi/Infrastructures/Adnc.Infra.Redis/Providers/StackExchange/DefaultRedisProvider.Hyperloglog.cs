using Adnc.Infra.Redis.Core;
using StackExchange.Redis;

namespace Adnc.Infra.Redis.Providers.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : IRedisProvider
    {
        public bool PfAdd<T>(string cacheKey, List<T> values)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(values, nameof(values));

            var list = new List<RedisValue>();

            foreach (var item in values)
            {
                list.Add(_serializer.Serialize(item));
            }

            var res = _redisDb.HyperLogLogAdd(cacheKey, list.ToArray());
            return res;
        }

        public async Task<bool> PfAddAsync<T>(string cacheKey, List<T> values)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(values, nameof(values));

            var list = new List<RedisValue>();

            foreach (var item in values)
            {
                list.Add(_serializer.Serialize(item));
            }

            var res = await _redisDb.HyperLogLogAddAsync(cacheKey, list.ToArray());
            return res;
        }

        public long PfCount(List<string> cacheKeys)
        {
            ArgumentCheck.NotNullAndCountGTZero(cacheKeys, nameof(cacheKeys));

            var list = new List<RedisKey>();

            foreach (var item in cacheKeys)
            {
                list.Add(item);
            }

            var res = _redisDb.HyperLogLogLength(list.ToArray());
            return res;
        }

        public async Task<long> PfCountAsync(List<string> cacheKeys)
        {
            ArgumentCheck.NotNullAndCountGTZero(cacheKeys, nameof(cacheKeys));

            var list = new List<RedisKey>();

            foreach (var item in cacheKeys)
            {
                list.Add(item);
            }

            var res = await _redisDb.HyperLogLogLengthAsync(list.ToArray());
            return res;
        }

        public bool PfMerge(string destKey, List<string> sourceKeys)
        {
            ArgumentCheck.NotNullOrWhiteSpace(destKey, nameof(destKey));
            ArgumentCheck.NotNullAndCountGTZero(sourceKeys, nameof(sourceKeys));

            var list = new List<RedisKey>();

            foreach (var item in sourceKeys)
            {
                list.Add(item);
            }

            _redisDb.HyperLogLogMerge(destKey, list.ToArray());
            return true;
        }

        public async Task<bool> PfMergeAsync(string destKey, List<string> sourceKeys)
        {
            ArgumentCheck.NotNullOrWhiteSpace(destKey, nameof(destKey));
            ArgumentCheck.NotNullAndCountGTZero(sourceKeys, nameof(sourceKeys));

            var list = new List<RedisKey>();

            foreach (var item in sourceKeys)
            {
                list.Add(item);
            }

            await _redisDb.HyperLogLogMergeAsync(destKey, list.ToArray());
            return true;
        }
    }
}