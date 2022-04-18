using Adnc.Application.Shared.Caching;
using Adnc.Infra.Caching;
using System;
using System.Threading.Tasks;

namespace Adnc.Whse.Application.Services.Caching
{
    public class CacheService : AbstractCacheService
    {
        private readonly Lazy<ICacheProvider> _cache;

        public CacheService(Lazy<ICacheProvider> cache)
            : base(cache)
            => _cache = cache;

        public override async Task PreheatAsync()
        {
            // TODO
            await Task.CompletedTask;
        }
    }
}