using Adnc.Infra.Redis.Core;
using Adnc.Infra.Redis.Core.Internal;

namespace StackExchange.Redis
{
    public static class IDatabaseExtension
    {
        #region Distributed Locker

        public static (bool Success, string LockValue) Lock(this IDatabase redisDb, string cacheKey, int timeoutSeconds = 5, bool autoDelay = false)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotLessThanOrEqualZero(timeoutSeconds, nameof(timeoutSeconds));

            var lockKey = GetLockKey(cacheKey);
            var lockValue = Guid.NewGuid().ToString();
            var timeoutMilliseconds = timeoutSeconds * 1000;
            var expiration = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            var flag = redisDb.StringSet(lockKey, lockValue, expiration, When.NotExists);
            if (flag && autoDelay)
            {
                var refreshMilliseconds = (int)(timeoutMilliseconds / 2.0);
                var autoDelayTimer = new Timer(timerState => Delay(redisDb, lockKey, lockValue, timeoutMilliseconds), null, refreshMilliseconds, refreshMilliseconds);
                var addResult = AutoDelayTimers.Instance.TryAdd(lockKey, autoDelayTimer);
                if (!addResult)
                {
                    autoDelayTimer?.Dispose();
                    redisDb.SafedUnLock(cacheKey, lockValue);
                    return (false, string.Empty);
                }
            }
            return (flag, flag ? lockValue : string.Empty);
        }

        public static async Task<(bool Success, string LockValue)> LockAsync(this IDatabase redisDb, string cacheKey, int timeoutSeconds = 5, bool autoDelay = false)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotLessThanOrEqualZero(timeoutSeconds, nameof(timeoutSeconds));

            var lockKey = GetLockKey(cacheKey);
            var lockValue = Guid.NewGuid().ToString();
            var timeoutMilliseconds = timeoutSeconds * 1000;
            var expiration = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            bool flag = await redisDb.StringSetAsync(lockKey, lockValue, expiration, When.NotExists);
            if (flag && autoDelay)
            {
                var refreshMilliseconds = (int)(timeoutMilliseconds / 2.0);
                var autoDelayTimer = new Timer(timerState => Delay(redisDb, lockKey, lockValue, timeoutMilliseconds), null, refreshMilliseconds, refreshMilliseconds);
                var addResult = AutoDelayTimers.Instance.TryAdd(lockKey, autoDelayTimer);
                if (!addResult)
                {
                    autoDelayTimer?.Dispose();
                    await redisDb.SafedUnLockAsync(cacheKey, lockValue);
                    return (false, string.Empty);
                }
            }
            return (flag, flag ? lockValue : string.Empty);
        }

        public static bool SafedUnLock(this IDatabase redisDb, string cacheKey, string lockValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(lockValue, nameof(lockValue));

            var lockKey = GetLockKey(cacheKey);
            AutoDelayTimers.Instance.CloseTimer(lockKey);

            var script = @"local invalue = @value
                                    local currvalue = redis.call('get',@key)
                                    if(invalue==currvalue) then redis.call('del',@key)
                                        return 1
                                    else
                                        return 0
                                    end";
            var parameters = new { key = lockKey, value = lockValue };
            var prepared = LuaScript.Prepare(script);
            var result = (int)redisDb.ScriptEvaluateAsync(prepared, parameters).GetAwaiter().GetResult();
            return result == 1;
        }

        public static async Task<bool> SafedUnLockAsync(this IDatabase redisDb, string cacheKey, string lockValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(lockValue, nameof(lockValue));

            var lockKey = GetLockKey(cacheKey);
            AutoDelayTimers.Instance.CloseTimer(lockKey);

            var script = @"local invalue = @value
                                    local currvalue = redis.call('get',@key)
                                    if(invalue==currvalue) then redis.call('del',@key)
                                        return 1
                                    else
                                        return 0
                                    end";
            var parameters = new { key = lockKey, value = lockValue };
            var prepared = LuaScript.Prepare(script);
            var result = (int)await redisDb.ScriptEvaluateAsync(prepared, parameters);
            return result == 1;
        }

        internal static void Delay(IDatabase redisDb, string key, string value, int milliseconds)
        {
            if (!AutoDelayTimers.Instance.ContainsKey(key))
                return;

            // local ttltime = redis.call('PTTL', @key)
            var script = @"local val = redis.call('GET', @key)
                                    if val==@value then
                                        redis.call('PEXPIRE', @key, @milliseconds)
                                        return 1
                                    end
                                    return 0";
            object parameters = new { key, value, milliseconds };
            var prepared = LuaScript.Prepare(script);
            var result = redisDb.ScriptEvaluateAsync(prepared, parameters, CommandFlags.None).GetAwaiter().GetResult();
            if ((int)result == 0)
            {
                AutoDelayTimers.Instance.CloseTimer(key);
            }
            return;
        }

        internal static string GetLockKey(string cacheKey)
        {
            return $"adnc:locker:{cacheKey.Replace(":", "-")}";
        }

        #endregion Distributed Locker

        #region Expire keys

        public static async Task KeyExpireAsync(this IDatabase redisDb, IEnumerable<string> cacheKeys, int seconds)
        {
            ArgumentCheck.NotNullAndCountGTZero(cacheKeys, nameof(cacheKeys));

            var script = @"for i, inkey in ipairs(KEYS) do
                                       redis.call('EXPIRE',inkey,ARGV[1])
                                    end ";
            var keys = Array.ConvertAll(cacheKeys.ToArray(), item => (RedisKey)item);
            var values = new RedisValue[] { seconds };
            var result = await redisDb.ScriptEvaluateAsync(script, keys, values);
        }

        #endregion Expire keys

        #region Bloom Filter

        public static async Task BfReserveAsync(this IDatabase redisDb, RedisKey key, double errorRate, int initialCapacity)
            => await redisDb.ExecuteAsync("BF.RESERVE", key, errorRate, initialCapacity);

        public static async Task<bool> BfAddAsync(this IDatabase redisDb, RedisKey key, RedisValue value)
            => (bool)await redisDb.ExecuteAsync("BF.ADD", key, value);

        public static async Task<bool[]> BfAddAsync(this IDatabase redisDb, RedisKey key, IEnumerable<RedisValue> values)
            => (bool[]?)await redisDb.ExecuteAsync("BF.MADD", values.Cast<object>().Prepend(key).ToArray()) ?? Array.Empty<bool>();

        public static async Task<bool> BfExistsAsync(this IDatabase redisDb, RedisKey key, RedisValue value)
            => (bool)await redisDb.ExecuteAsync("BF.EXISTS", key, value);

        public static async Task<bool[]> BfExistsAsync(this IDatabase redisDb, RedisKey key, IEnumerable<RedisValue> values)
            => (bool[]?)await redisDb.ExecuteAsync("BF.MEXISTS", values.Cast<object>().Prepend(key).ToArray()) ?? Array.Empty<bool>();

        #endregion Bloom Filter

        #region TopK

        public static async Task TopKReserveAsync(this IDatabase db, RedisKey key, int topK, int width = 8, int depth = 7, double decay = 0.9)
            => await db.ExecuteAsync("TOPK.RESERVE", key, topK, width, depth, decay);

        public static async Task<RedisValue> TopKAddAsync(this IDatabase db, RedisKey key, RedisValue value)
            => (RedisValue)await db.ExecuteAsync("TOPK.ADD", key, value);

        public static async Task<RedisValue[]> TopKAddAsync(this IDatabase db, RedisKey key, IEnumerable<RedisValue> values)
            => (RedisValue[]?)await db.ExecuteAsync("TOPK.ADD", values.Cast<object>().Prepend(key).ToArray()) ?? Array.Empty<RedisValue>();

        public static async Task<RedisValue> TopKIncrementAsync(this IDatabase db, RedisKey key, RedisValue value, int increment)
            => (RedisValue)await db.ExecuteAsync("TOPK.INCRBY", key, value, increment);

        public static async Task<RedisValue[]> TopKIncrementAsync(this IDatabase db, RedisKey key, (RedisValue value, int increment)[] increments)
            => (RedisValue[]?)await db.ExecuteAsync("TOPK.INCRBY", increments.SelectMany(i => new object[] { i.value, i.increment }).Prepend(key).ToArray()) ?? Array.Empty<RedisValue>();

        public static async Task<RedisValue[]> TopKListAsync(this IDatabase db, RedisKey key)
            => (RedisValue[]?)await db.ExecuteAsync("TOPK.LIST", key) ?? Array.Empty<RedisValue>();

        public static async Task<bool> TopKQueryAsync(this IDatabase db, RedisKey key, RedisValue value)
            => (bool)await db.ExecuteAsync("TOPK.QUERY", key, value);

        public static async Task<bool[]> TopKQueryAsync(this IDatabase db, RedisKey key, RedisValue[] values)
            => (bool[]?)await db.ExecuteAsync("TOPK.QUERY", values.Cast<object>().Prepend(key).ToArray()) ?? Array.Empty<bool>();

        public static async Task<int> TopKCountAsync(this IDatabase db, RedisKey key, RedisValue value)
            => (int)await db.ExecuteAsync("TOPK.COUNT", key, value);

        public static async Task<int[]> TopKCountAsync(this IDatabase db, RedisKey key, RedisValue[] values)
            => (int[]?)await db.ExecuteAsync("TOPK.COUNT", values.Cast<object>().Prepend(key).ToArray()) ?? Array.Empty<int>();

        #endregion TopK
    }
}