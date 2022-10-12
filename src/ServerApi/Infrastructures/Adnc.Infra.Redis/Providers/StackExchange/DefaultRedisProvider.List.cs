using Adnc.Infra.Redis.Core;
using StackExchange.Redis;

namespace Adnc.Infra.Redis.Providers.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : IRedisProvider
    {
        public T LIndex<T>(string cacheKey, long index)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _redisDb.ListGetByIndex(cacheKey, index);
            return _serializer.Deserialize<T>(bytes);
        }

        public long LLen(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return _redisDb.ListLength(cacheKey);
        }

        public T LPop<T>(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _redisDb.ListLeftPop(cacheKey);
            return _serializer.Deserialize<T>(bytes);
        }

        public long LPush<T>(string cacheKey, IList<T> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(cacheValues, nameof(cacheValues));

            var list = new List<RedisValue>();

            foreach (var item in cacheValues)
            {
                list.Add(_serializer.Serialize(item));
            }

            var len = _redisDb.ListLeftPush(cacheKey, list.ToArray());
            return len;
        }

        public List<T> LRange<T>(string cacheKey, long start, long stop)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var bytes = _redisDb.ListRange(cacheKey, start, stop);

            foreach (var item in bytes)
            {
                list.Add(_serializer.Deserialize<T>(item));
            }

            return list;
        }

        public long LRem<T>(string cacheKey, long count, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);
            return _redisDb.ListRemove(cacheKey, bytes, count);
        }

        public bool LSet<T>(string cacheKey, long index, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);
            _redisDb.ListSetByIndex(cacheKey, index, bytes);
            return true;
        }

        public bool LTrim(string cacheKey, long start, long stop)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            _redisDb.ListTrim(cacheKey, start, stop);
            return true;
        }

        public long LPushX<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);
            return _redisDb.ListLeftPush(cacheKey, bytes);
        }

        public long LInsertBefore<T>(string cacheKey, T pivot, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var pivotBytes = _serializer.Serialize(pivot);
            var cacheValueBytes = _serializer.Serialize(cacheValue);
            return _redisDb.ListInsertBefore(cacheKey, pivotBytes, cacheValueBytes);
        }

        public long LInsertAfter<T>(string cacheKey, T pivot, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var pivotBytes = _serializer.Serialize(pivot);
            var cacheValueBytes = _serializer.Serialize(cacheValue);
            return _redisDb.ListInsertAfter(cacheKey, pivotBytes, cacheValueBytes);
        }

        public long RPushX<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);
            return _redisDb.ListRightPush(cacheKey, bytes);
        }

        public long RPush<T>(string cacheKey, IList<T> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(cacheValues, nameof(cacheValues));

            var list = new List<RedisValue>();

            foreach (var item in cacheValues)
            {
                list.Add(_serializer.Serialize(item));
            }

            var len = _redisDb.ListRightPush(cacheKey, list.ToArray());
            return len;
        }

        public T RPop<T>(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _redisDb.ListRightPop(cacheKey);
            return _serializer.Deserialize<T>(bytes);
        }

        public async Task<T> LIndexAsync<T>(string cacheKey, long index)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = await _redisDb.ListGetByIndexAsync(cacheKey, index);
            return _serializer.Deserialize<T>(bytes);
        }

        public async Task<long> LLenAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            return await _redisDb.ListLengthAsync(cacheKey);
        }

        public async Task<T> LPopAsync<T>(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = await _redisDb.ListLeftPopAsync(cacheKey);
            return _serializer.Deserialize<T>(bytes);
        }

        public async Task<long> LPushAsync<T>(string cacheKey, IList<T> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(cacheValues, nameof(cacheValues));

            var list = new List<RedisValue>();

            foreach (var item in cacheValues)
            {
                list.Add(_serializer.Serialize(item));
            }

            var len = await _redisDb.ListLeftPushAsync(cacheKey, list.ToArray());
            return len;
        }

        public async Task<List<T>> LRangeAsync<T>(string cacheKey, long start, long stop)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var bytes = await _redisDb.ListRangeAsync(cacheKey, start, stop);

            foreach (var item in bytes)
            {
                list.Add(_serializer.Deserialize<T>(item));
            }

            return list;
        }

        public async Task<long> LRemAsync<T>(string cacheKey, long count, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);
            return await _redisDb.ListRemoveAsync(cacheKey, bytes, count);
        }

        public async Task<bool> LSetAsync<T>(string cacheKey, long index, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);
            await _redisDb.ListSetByIndexAsync(cacheKey, index, bytes);
            return true;
        }

        public async Task<bool> LTrimAsync(string cacheKey, long start, long stop)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            await _redisDb.ListTrimAsync(cacheKey, start, stop);
            return true;
        }

        public Task<long> LPushXAsync<T>(string cacheKey, T cacheValue)
        {
            //ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            //var bytes = _serializer.Serialize(cacheValue);
            //return await _cache.ListLeftPushAsync(cacheKey, bytes);
            throw new NotImplementedException();
        }

        public async Task<long> LInsertBeforeAsync<T>(string cacheKey, T pivot, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var pivotBytes = _serializer.Serialize(pivot);
            var cacheValueBytes = _serializer.Serialize(cacheValue);
            return await _redisDb.ListInsertBeforeAsync(cacheKey, pivotBytes, cacheValueBytes);
        }

        public async Task<long> LInsertAfterAsync<T>(string cacheKey, T pivot, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var pivotBytes = _serializer.Serialize(pivot);
            var cacheValueBytes = _serializer.Serialize(cacheValue);
            return await _redisDb.ListInsertAfterAsync(cacheKey, pivotBytes, cacheValueBytes);
        }

        public Task<long> RPushXAsync<T>(string cacheKey, T cacheValue)
        {
            //ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            //var bytes = _serializer.Serialize(cacheValue);
            //return await _cache.ListRightPushAsync(cacheKey, bytes);
            throw new NotImplementedException();
        }

        public async Task<long> RPushAsync<T>(string cacheKey, IList<T> cacheValues)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(cacheValues, nameof(cacheValues));

            var list = new List<RedisValue>();

            foreach (var item in cacheValues)
            {
                list.Add(_serializer.Serialize(item));
            }

            var len = await _redisDb.ListRightPushAsync(cacheKey, list.ToArray());
            return len;
        }

        public async Task<T> RPopAsync<T>(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = await _redisDb.ListRightPopAsync(cacheKey);
            return _serializer.Deserialize<T>(bytes);
        }
    }
}