using System;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Infra.Caching.Core;

namespace StackExchange.Redis
{
    public static class IDatabaseExtension
    {
        public static (bool Success, string LockValue) Lock(this IDatabase redisDb, string cacheKey, int timeoutSeconds = 5, bool autoDelay = true)
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
                    redisDb.SafedUnLock(lockKey, lockValue);
                    return (false, null);
                }
            }
            return (flag, flag ? lockValue : null);
        }

        public static async Task<(bool Success, string LockValue)> LockAsync(this IDatabase redisDb, string cacheKey, int timeoutSeconds = 5, bool autoDelay = true)
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
                    await redisDb.SafedUnLockAsync(lockKey, lockValue);
                    return (false, null);
                }
            }
            return (flag, flag ? lockValue : null);
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
    }
}
