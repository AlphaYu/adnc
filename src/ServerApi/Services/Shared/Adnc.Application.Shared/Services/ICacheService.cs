using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Infra.Caching;
using Adnc.Infra.Mapper;

namespace Adnc.Application.Shared.Services
{
    public interface ICacheService
    {
        IObjectMapper Mapper { get; set; }
    }

    public abstract class AbstractCacheService : ICacheService
    {
        private readonly IRedisDistributedCache _cache;
        private readonly Lazy<IRedisProvider> _redisProvider;

        public AbstractCacheService(IRedisDistributedCache cache, Lazy<IRedisProvider> redisProvider)
        {
            _cache = cache;
            _redisProvider = redisProvider;
        }

        internal IRedisProvider RedisProvider => _redisProvider.Value;

        public IObjectMapper Mapper { get; set; }

        public async Task PreRemove(params string[] removingKeys)
        {
            var keys = string.Join(",", removingKeys);
            var preCacheKey = string.Format(BaseEasyCachingConsts.PreRemoveKey, keys.GetHashCode());
            await _cache.SetAsync(preCacheKey, keys, TimeSpan.FromSeconds(BaseEasyCachingConsts.OneDay));
            return;
        }

        public async Task PostRemove(params string[] removingKeys)
        {
            var keys = string.Join(",", removingKeys);
            var preCacheKey = string.Format(BaseEasyCachingConsts.PreRemoveKey, keys.GetHashCode());

            var keysList = new List<string>(removingKeys)
            {
                preCacheKey
            };

            await _cache.RemoveAllAsync(keysList);
            return;
        }
    }
}
