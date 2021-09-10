using Adnc.Application.Shared.BloomFilter;
using Adnc.Infra.Caching;
using Adnc.Infra.IRepositories;
using Adnc.Usr.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Adnc.Usr.Application.BloomFilter
{
    public class BloomFilterAccount : AbstractBloomFilter
    {
        private readonly Lazy<IServiceProvider> _services;

        public BloomFilterAccount(Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker
            , Lazy<IServiceProvider> services)
            : base(redisProvider, distributedLocker)
           => _services = services;

        public override string Name => "adnc:usr:bloomfilter:accouts";

        public override double ErrorRate => 0.001;

        public override int Capacity => 10000000;

        public override async Task InitAsync()
        {
            var exists = await ExistsBloomFilterAsync();
            if (!exists)
            {
                using var scope = _services.Value.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysUser>>();
                var values = await repository.GetAll()
                                                               .Select(x => x.Account)
                                                               .ToListAsync();
                await InitAsync(values);
            }
        }
    }
}