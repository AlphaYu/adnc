using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Adnc.Infra.Caching.Core;

namespace Adnc.Infra.Caching.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider: Adnc.Infra.Caching.IDistributedLocker
    {
        public bool Lock(string cacheKey, string cacheValue, TimeSpan? expiration = null)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(cacheValue, nameof(cacheValue));

            if (expiration == null) expiration = TimeSpan.FromMilliseconds(_cacheOptions.LockMs);

            bool flag = _redisDb.StringSet(cacheKey, cacheValue, expiration, When.NotExists);
            return flag;
        }

        public async Task<bool> LockAsync(string cacheKey, string cacheValue, TimeSpan? expiration = null)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(cacheValue, nameof(cacheValue));

            if (expiration == null) expiration = TimeSpan.FromMilliseconds(_cacheOptions.LockMs);

            bool flag = await _redisDb.StringSetAsync(cacheKey, cacheValue, expiration, When.NotExists);
            return flag;
        }

        public bool SafedUnLock(string cacheKey, string cacheValue)
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
            var result = (int) _redisDb.ScriptEvaluateAsync(prepared, parameters).GetAwaiter().GetResult();
            return result == 1;
        }

        public async Task<bool> SafedUnLockAsync(string cacheKey, string cacheValue)
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
            var result = (int)await _redisDb.ScriptEvaluateAsync(prepared, parameters);
            return result == 1;
        }
    }
}
