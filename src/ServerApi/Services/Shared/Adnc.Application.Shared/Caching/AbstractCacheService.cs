using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Core;
using Adnc.Infra.Mapper;
using Adnc.Application.Shared.Consts;

namespace Adnc.Application.Shared.Caching
{
    public interface ICacheService { }

    public abstract class AbstractCacheService : ICacheService
    {
        private readonly Lazy<ICacheProvider> _cache;
        private readonly Lazy<IRedisProvider> _redisProvider;

        public AbstractCacheService(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider)
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

        public string ConcatCacheKey(params object[] items)
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
    }
}
