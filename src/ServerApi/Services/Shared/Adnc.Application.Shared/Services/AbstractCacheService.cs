using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Core;
using Adnc.Infra.Mapper;
using Adnc.Infra.Common.Extensions;
using Adnc.Application.Shared.Consts;

namespace Adnc.Application.Shared.Services
{
    public interface ICacheService
    {
        IObjectMapper Mapper { get; set; }
    }

    public abstract class AbstractCacheService : ICacheService
    {
        private readonly Lazy<IRedisDistributedCache> _cache;
        private readonly Lazy<IRedisProvider> _redisProvider;
        private static int _workerId = 0;

        public AbstractCacheService(Lazy<IRedisDistributedCache> cache, Lazy<IRedisProvider> redisProvider)
        {
            _cache = cache;
            _redisProvider = redisProvider;
        }

        internal IRedisProvider RedisProvider => _redisProvider.Value;

        public IObjectMapper Mapper { get; set; }

        public string GetPreRemoveKey(IEnumerable<string> cacheKeys)
        {
            return $"{CachingConstValue.PreRemoveKey}{SharedCachingConsts.LinkChar}{string.Join(",", cacheKeys).GetHashCode()}";
        }

        public string ConcatCacheKey(params string[] items)
        {
            if (items == null || items.Length == 0)
                return string.Empty;

            var sbuilder = new StringBuilder();
            int index = 0;
            int total = items.Length;
            foreach (var item in items)
            {
                index++;
                sbuilder.Append(item);
                if (index != total)
                    sbuilder.Append(SharedCachingConsts.LinkChar);
            }
            return sbuilder.ToString();
        }

        public async Task RemoveCachesAsync(Func<Task> dataOperater, params string[] cacheKeys)
        {
            var preRemoveKey = GetPreRemoveKey(cacheKeys);
            await _cache.Value.SetAsync(preRemoveKey, cacheKeys, TimeSpan.FromSeconds(SharedCachingConsts.OneDay));

            await dataOperater();

            var needRemovedKeys = cacheKeys.Append(preRemoveKey);
            await _cache.Value.RemoveAllAsync(needRemovedKeys);
        }

        public async Task<int> GetWorkerIdAsync(string serverName)
        {
            if (_workerId > 0) return _workerId;

            var workerIdSortedSetCacheKey = string.Format(SharedCachingConsts.WorkerIdSortedSetCacheKey, serverName);

            if (!_redisProvider.Value.KeyExists(workerIdSortedSetCacheKey))
            {
                var lockKey = $"{workerIdSortedSetCacheKey}_lock";
                var lockValue = DateTime.Now.GetTotalMilliseconds().ToString();
                var flag = await _redisProvider.Value.StringSetAsync(lockKey, lockValue, TimeSpan.FromMilliseconds(5000), "nx");

                if (!flag)
                {
                    await Task.Delay(300);
                    return await GetWorkerIdAsync(serverName);
                }

                var set = new Dictionary<long, double>();
                for (long index = 0; index < 64; index++)
                {
                    set.Add(index, DateTime.Now.GetTotalMicroseconds());
                }
                await _redisProvider.Value.ZAddAsync(workerIdSortedSetCacheKey, set);

                await _redisProvider.Value.KeyDelAsync(lockKey);
            }

            var scirpt = @"local workerids = redis.call('ZRANGE', @key, @start,@stop)
                                    redis.call('ZADD',@key,@score,workerids[1])
                                    return workerids[1]";
            var parameters = new { key = workerIdSortedSetCacheKey, start = 0, stop = 0, score = DateTime.Now.GetTotalMicroseconds() };
            var luaResult = (byte[])_redisProvider.Value.ScriptEvaluate(scirpt, parameters);
            var workerId = _cache.Value.Serializer.Deserialize<long>(luaResult);
            return (int)workerId;
        }
    }
}
