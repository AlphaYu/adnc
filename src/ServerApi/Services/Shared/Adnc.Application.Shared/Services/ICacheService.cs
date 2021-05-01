using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Core;
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

        public string GetPreRemoveKey(IEnumerable<string> cacheKeys)
        {
            return $"{CachingConstValue.PreRemoveKey}{BaseEasyCachingConsts.LinkChar}{string.Join(",", cacheKeys).GetHashCode()}";
        }
    }
}
