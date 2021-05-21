using Adnc.Application.Shared.Caching;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Caching;
using Adnc.Usr.Application.Contracts.Consts;
using Adnc.Usr.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adnc.Usr.Application.Caching
{
    public class BloomFilterCacheKey : AbstractBloomFilter
    {
        private readonly Lazy<ICacheProvider> _cache;
        private readonly Lazy<IDistributedLocker> _distributedLocker;

        //private readonly Lazy<IRedisProvider> _redisProvider;
        private readonly Lazy<IServiceProvider> _services;

        public BloomFilterCacheKey(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker
            , Lazy<IServiceProvider> services)
            : base(cache, redisProvider, distributedLocker)
        {
            _cache = cache;
            _distributedLocker = distributedLocker;
            _services = services;

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
                     CachingConsts.MenuListCacheKey
                    ,CachingConsts.MenuTreeListCacheKey
                    ,CachingConsts.MenuRelationCacheKey
                    ,CachingConsts.MenuCodesCacheKey
                    ,CachingConsts.DetpListCacheKey
                    ,CachingConsts.DetpTreeListCacheKey
                    ,CachingConsts.DetpSimpleTreeListCacheKey
                    ,CachingConsts.RoleListCacheKey
                };

                var ids = new List<long>();
                using (var scope = _services.Value.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysUser>>();
                    ids = await repository.GetAll().Select(x => x.Id).ToListAsync();
                }

                if (ids?.Any() == true)
                    values.AddRange(ids.Select(x => string.Concat(CachingConsts.UserValidateInfoKeyPrefix, CachingConsts.LinkChar, x)));

                return values;
            });
        }
    }
}