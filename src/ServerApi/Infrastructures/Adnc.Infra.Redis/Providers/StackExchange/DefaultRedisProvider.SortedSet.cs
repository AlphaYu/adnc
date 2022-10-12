using Adnc.Infra.Redis.Core;
using StackExchange.Redis;

namespace Adnc.Infra.Redis.Providers.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : IRedisProvider
    {
        public long ZAdd<T>(string cacheKey, Dictionary<T, double> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var param = new List<SortedSetEntry>();

            foreach (var item in cacheValues)
            {
                param.Add(new SortedSetEntry(_serializer.Serialize(item.Key), item.Value));
            }

            var len = _redisDb.SortedSetAdd(cacheKey, param.ToArray());

            return len;
        }

        public long ZCard(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = _redisDb.SortedSetLength(cacheKey);
            return len;
        }

        public long ZCount(string cacheKey, double min, double max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = _redisDb.SortedSetLengthByValue(cacheKey, min, max);
            return len;
        }

        public double ZIncrBy(string cacheKey, string field, double val = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(field, nameof(field));

            var value = _redisDb.SortedSetIncrement(cacheKey, field, val);
            return value;
        }

        public long ZLexCount(string cacheKey, string min, string max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = _redisDb.SortedSetLengthByValue(cacheKey, min, max);
            return len;
        }

        public List<T> ZRange<T>(string cacheKey, long start, long stop)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var bytes = _redisDb.SortedSetRangeByRank(cacheKey, start, stop);

            foreach (var item in bytes)
            {
                list.Add(_serializer.Deserialize<T>(item));
            }

            return list;
        }

        public long? ZRank<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);

            var index = _redisDb.SortedSetRank(cacheKey, bytes);

            return index;
        }

        public long ZRem<T>(string cacheKey, IList<T> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = new List<RedisValue>();

            foreach (var item in cacheValues)
            {
                bytes.Add(_serializer.Serialize(item));
            }

            var len = _redisDb.SortedSetRemove(cacheKey, bytes.ToArray());

            return len;
        }

        public double? ZScore<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);

            var score = _redisDb.SortedSetScore(cacheKey, bytes);

            return score;
        }

        public async Task<long> ZAddAsync<T>(string cacheKey, Dictionary<T, double> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var param = new List<SortedSetEntry>();

            foreach (var item in cacheValues)
            {
                param.Add(new SortedSetEntry(_serializer.Serialize(item.Key), item.Value));
            }

            var len = await _redisDb.SortedSetAddAsync(cacheKey, param.ToArray());

            return len;
        }

        public async Task<long> ZCardAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = await _redisDb.SortedSetLengthAsync(cacheKey);
            return len;
        }

        public async Task<long> ZCountAsync(string cacheKey, double min, double max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = await _redisDb.SortedSetLengthByValueAsync(cacheKey, min, max);
            return len;
        }

        public async Task<double> ZIncrByAsync(string cacheKey, string field, double val = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(field, nameof(field));

            var value = await _redisDb.SortedSetIncrementAsync(cacheKey, field, val);
            return value;
        }

        public async Task<long> ZLexCountAsync(string cacheKey, string min, string max)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = await _redisDb.SortedSetLengthByValueAsync(cacheKey, min, max);
            return len;
        }

        public async Task<List<T>> ZRangeAsync<T>(string cacheKey, long start, long stop)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var bytes = await _redisDb.SortedSetRangeByRankAsync(cacheKey, start, stop);

            foreach (var item in bytes)
            {
                list.Add(_serializer.Deserialize<T>(item));
            }

            return list;
        }

        public async Task<long?> ZRankAsync<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);

            var index = await _redisDb.SortedSetRankAsync(cacheKey, bytes);

            return index;
        }

        public async Task<long> ZRemAsync<T>(string cacheKey, IList<T> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = new List<RedisValue>();

            foreach (var item in cacheValues)
            {
                bytes.Add(_serializer.Serialize(item));
            }

            var len = await _redisDb.SortedSetRemoveAsync(cacheKey, bytes.ToArray());

            return len;
        }

        public async Task<double?> ZScoreAsync<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);

            var score = await _redisDb.SortedSetScoreAsync(cacheKey, bytes);

            return score;
        }
    }
}