using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Adnc.Infra.Caching;
using Adnc.Maint.Application.Contracts.Consts;
using Adnc.Application.Shared.Caching;

namespace Adnc.Maint.Application.Caching
{
    public class BloomFilterCacheKey : AbstractBloomFilter
    {
        //private readonly Lazy<ICacheProvider> _cache;
        //private readonly Lazy<IDistributedLocker> _distributedLocker;
        //private readonly Lazy<IRedisProvider> _redisProvider;
        //private readonly Lazy<IServiceProvider> _services;

        public BloomFilterCacheKey(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker
            , Lazy<IServiceProvider> services)
            : base(cache, redisProvider, distributedLocker)
        {
            //_cache = cache;
            //_distributedLocker = distributedLocker;
            //_services = services;
            var setting = cache.Value.CacheOptions.PenetrationSetting.BloomFilterSetting;
            Name = setting.Name;
            ErrorRate = setting.ErrorRate;
            Capacity = setting.Capacity;
        }

        public override string Name { get; }

        public override double ErrorRate { get; }

        public override int Capacity { get; }

        public override async Task InitAsync()
        {
            await base.InitAsync(async () =>
            {
                var values = new List<string>()
                {
                     CachingConsts.CfgListCacheKey
                    ,CachingConsts.DictListCacheKey
                };

                return await Task.FromResult(values);
            });
        }
    }
}
