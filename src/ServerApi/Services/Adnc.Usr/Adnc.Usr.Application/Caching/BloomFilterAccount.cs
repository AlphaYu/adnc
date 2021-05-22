using Adnc.Application.Shared.Caching;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Caching;
using Adnc.Usr.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adnc.Usr.Application.Caching
{
    public class BloomFilterAccount : AbstractBloomFilter
    {
        private readonly Lazy<ICacheProvider> _cache;
        private readonly Lazy<IDistributedLocker> _distributedLocker;

        //private readonly Lazy<IRedisProvider> _redisProvider;
        private readonly Lazy<IServiceProvider> _services;

        public BloomFilterAccount(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker
            , Lazy<IServiceProvider> services)
            : base(cache, redisProvider, distributedLocker)
        {
            _cache = cache;
            _distributedLocker = distributedLocker;
            _services = services;
        }

        public override string Name => $"adnc:{nameof(BloomFilterAccount).ToLower()}";

        public override double ErrorRate => 0.001;

        public override int Capacity => 10000000;

        public override async Task InitAsync()
        {
            await base.InitAsync(async () =>
            {
                var accounts = new List<string>();
                using (var scope = _services.Value.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysUser>>();
                    accounts = await repository.GetAll().Select(x => x.Account).ToListAsync();
                }
                return accounts;
            });
        }
    }
}