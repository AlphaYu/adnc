using Adnc.Application.Shared.Caching;
using Adnc.Infra.Caching;
using System;
using System.Threading.Tasks;

namespace Adnc.Whse.Application.Services.Caching
{
    public class CacheService : AbstractCacheService
    {
        private readonly Lazy<ICacheProvider> _cache;

        //private readonly Lazy<IDistributedLocker> _distributedLocker;
        private readonly Lazy<IBloomFilterFactory> _bloomFilterFactory;

        public CacheService(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker
            , Lazy<IBloomFilterFactory> bloomFilterFactory)
            : base(cache, redisProvider, distributedLocker)
        {
            _cache = cache;
            _bloomFilterFactory = bloomFilterFactory;
        }

        internal (IBloomFilter CacheKeys, IBloomFilter Null) BloomFilters
        {
            get
            {
                var cacheFilter = _bloomFilterFactory.Value.GetBloomFilter(_cache.Value.CacheOptions.PenetrationSetting.BloomFilterSetting.Name);
                return (cacheFilter, null);
            }
        }

        public override async Task PreheatAsync()
        {
            //todo
            await Task.CompletedTask;
        }
    }
}