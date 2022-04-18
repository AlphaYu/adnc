using Adnc.Application.Shared.BloomFilter;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Configurations;
using Adnc.Shared.Consts.Caching.Maint;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Maint.Application.BloomFilter
{
    public class BloomFilterCacheKey : AbstractBloomFilter
    {
        private readonly Lazy<IServiceProvider> _services;
        private readonly CacheOptions.BloomFilterSetting _setting;

        public BloomFilterCacheKey(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker
            , Lazy<IServiceProvider> services)
            : base(redisProvider, distributedLocker)
        {
            _services = services;
            _setting = cache.Value.CacheOptions.PenetrationSetting.BloomFilterSetting;
        }

        public override string Name => _setting.Name;

        public override double ErrorRate => _setting.ErrorRate;

        public override int Capacity => _setting.Capacity;

        public override async Task InitAsync()
        {
            var exists = await ExistsBloomFilterAsync();
            if (!exists)
            {
                var values = new List<string>()
                {
                     CachingConsts.CfgListCacheKey
                    ,CachingConsts.DictListCacheKey
                };

                await InitAsync(values);
            }
        }
    }
}