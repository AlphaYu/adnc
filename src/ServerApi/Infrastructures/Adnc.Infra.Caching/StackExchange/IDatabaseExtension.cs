using System;
using System.Threading.Tasks;
using Adnc.Infra.Caching.Core;

namespace StackExchange.Redis
{
    public static class IDatabaseExtension
    {
        public static bool Lock(this IDatabase redisDb, string cacheKey, string cacheValue, TimeSpan? expiration = null)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(cacheValue, nameof(cacheValue));

            if (expiration == null) expiration = TimeSpan.FromMilliseconds(5000);

            bool flag = redisDb.StringSet(cacheKey, cacheValue, expiration, When.NotExists);
            return flag;
        }

        public static async Task<bool> LockAsync(this IDatabase redisDb, string cacheKey, string cacheValue, TimeSpan? expiration = null)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(cacheValue, nameof(cacheValue));

            if (expiration == null) expiration = TimeSpan.FromMilliseconds(5000);

            bool flag = await redisDb.StringSetAsync(cacheKey, cacheValue, expiration, When.NotExists);
            return flag;
        }

        public static bool SafedUnLock(this IDatabase redisDb, string cacheKey, string cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(cacheValue, nameof(cacheValue));
            var script = @"local invalue = @value 
                                    local currvalue = redis.call('get',@key) 
                                    if(invalue==currvalue) then redis.call('del',@key) 
                                        return 1
                                    else
                                        return 0
                                    end";
            var parameters = new { key = cacheKey, value = cacheValue };
            var prepared = LuaScript.Prepare(script);
            var result = (int)redisDb.ScriptEvaluateAsync(prepared, parameters).GetAwaiter().GetResult();
            return result == 1;
        }

        public static async Task<bool> SafedUnLockAsync(this IDatabase redisDb, string cacheKey, string cacheValue)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(cacheValue, nameof(cacheValue));
            var script = @"local invalue = @value 
                                    local currvalue = redis.call('get',@key) 
                                    if(invalue==currvalue) then redis.call('del',@key) 
                                        return 1
                                    else
                                        return 0
                                    end";
            var parameters = new { key = cacheKey, value = cacheValue };
            var prepared = LuaScript.Prepare(script);
            var result = (int)await redisDb.ScriptEvaluateAsync(prepared, parameters);
            return result == 1;
        }
    }
}
