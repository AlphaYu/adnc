using Adnc.Infra.Redis.Core;
using StackExchange.Redis;

namespace Adnc.Infra.Redis.Providers.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : IRedisProvider
    {
        public long SAdd<T>(string cacheKey, IList<T> cacheValues, TimeSpan? expiration = null)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(cacheValues, nameof(cacheValues));

            var list = new List<RedisValue>();

            foreach (var item in cacheValues)
            {
                list.Add(_serializer.Serialize(item));
            }

            var len = _redisDb.SetAdd(cacheKey, list.ToArray());

            if (expiration.HasValue)
            {
                _redisDb.KeyExpire(cacheKey, expiration.Value);
            }

            return len;
        }

        public long SCard(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = _redisDb.SetLength(cacheKey);
            return len;
        }

        public bool SIsMember<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);

            var flag = _redisDb.SetContains(cacheKey, bytes);
            return flag;
        }

        public List<T> SMembers<T>(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var bytes = _redisDb.SetMembers(cacheKey);

            foreach (var item in bytes)
            {
                list.Add(_serializer.Deserialize<T>(item));
            }

            return list;
        }

        public T SPop<T>(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _redisDb.SetPop(cacheKey);

            return _serializer.Deserialize<T>(bytes);
        }

        public List<T> SRandMember<T>(string cacheKey, int count = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var bytes = _redisDb.SetRandomMembers(cacheKey, count);

            foreach (var item in bytes)
            {
                list.Add(_serializer.Deserialize<T>(item));
            }

            return list;
        }

        public long SRem<T>(string cacheKey, IList<T>? cacheValues = null)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = 0L;

            if (cacheValues != null && cacheValues.Any())
            {
                var bytes = new List<RedisValue>();

                foreach (var item in cacheValues)
                {
                    bytes.Add(_serializer.Serialize<T>(item));
                }

                len = _redisDb.SetRemove(cacheKey, bytes.ToArray());
            }
            else
            {
                var flag = _redisDb.KeyDelete(cacheKey);
                len = flag ? 1 : 0;
            }

            return len;
        }

        public async Task<long> SAddAsync<T>(string cacheKey, IList<T> cacheValues, TimeSpan? expiration = null)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(cacheValues, nameof(cacheValues));

            var list = new List<RedisValue>();

            foreach (var item in cacheValues)
            {
                list.Add(_serializer.Serialize(item));
            }

            var len = await _redisDb.SetAddAsync(cacheKey, list.ToArray());

            if (expiration.HasValue)
            {
                await _redisDb.KeyExpireAsync(cacheKey, expiration.Value);
            }

            return len;
        }

        public async Task<long> SCardAsync(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = await _redisDb.SetLengthAsync(cacheKey);
            return len;
        }

        public async Task<bool> SIsMemberAsync<T>(string cacheKey, T cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = _serializer.Serialize(cacheValue);

            var flag = await _redisDb.SetContainsAsync(cacheKey, bytes);
            return flag;
        }

        public async Task<List<T>> SMembersAsync<T>(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var vals = await _redisDb.SetMembersAsync(cacheKey);

            foreach (var item in vals)
            {
                list.Add(_serializer.Deserialize<T>(item));
            }

            return list;
        }

        public async Task<T> SPopAsync<T>(string cacheKey)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var bytes = await _redisDb.SetPopAsync(cacheKey);

            return _serializer.Deserialize<T>(bytes);
        }

        public async Task<List<T>> SRandMemberAsync<T>(string cacheKey, int count = 1)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var list = new List<T>();

            var bytes = await _redisDb.SetRandomMembersAsync(cacheKey, count);

            foreach (var item in bytes)
            {
                list.Add(_serializer.Deserialize<T>(item));
            }

            return list;
        }

        public async Task<long> SRemAsync<T>(string cacheKey, IList<T>? cacheValues = null)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));

            var len = 0L;

            if (cacheValues != null && cacheValues.Any())
            {
                var bytes = new List<RedisValue>();

                foreach (var item in cacheValues)
                {
                    bytes.Add(_serializer.Serialize<T>(item));
                }

                len = await _redisDb.SetRemoveAsync(cacheKey, bytes.ToArray());
            }
            else
            {
                var flag = await _redisDb.KeyDeleteAsync(cacheKey);
                len = flag ? 1 : 0;
            }

            return len;
        }
    }
}