using Adnc.Application.Shared.BloomFilter;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Configurations;
using Adnc.Infra.IRepositories;
using Adnc.Shared.Consts.Caching.Usr;
using Adnc.Usr.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adnc.Usr.Application.BloomFilter
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
                    CachingConsts.MenuListCacheKey,
                    CachingConsts.MenuTreeListCacheKey,
                    CachingConsts.MenuRelationCacheKey,
                    CachingConsts.MenuCodesCacheKey,
                    CachingConsts.DetpListCacheKey,
                    CachingConsts.DetpTreeListCacheKey,
                    CachingConsts.DetpSimpleTreeListCacheKey,
                    CachingConsts.RoleListCacheKey
                };

                using var scope = _services.Value.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysUser>>();
                var ids = await repository
                                                        .GetAll()
                                                        .Select(x => x.Id)
                                                        .ToListAsync();
                if (ids.IsNotNullOrEmpty())
                    values.AddRange(ids.Select(x => string.Concat(CachingConsts.UserValidateInfoKeyPrefix, CachingConsts.LinkChar, x)));

                await InitAsync(values);
            }
        }
    }
}