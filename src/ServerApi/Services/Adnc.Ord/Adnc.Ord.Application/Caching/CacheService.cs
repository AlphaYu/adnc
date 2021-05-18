﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Caching;
using Adnc.Ord.Core.Entities;
using Adnc.Ord.Application.Contracts.Dtos;
using Adnc.Ord.Application.Contracts.Consts;
using Adnc.Application.Shared.Caching;

namespace Adnc.Maint.Application.Services.Caching
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
