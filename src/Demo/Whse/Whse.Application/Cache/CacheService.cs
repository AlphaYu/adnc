using Adnc.Infra.Redis.Caching.Core.Preheater;

namespace Adnc.Demo.Whse.Application.Cache;

public class CacheService(Lazy<ICacheProvider> cacheProvider, Lazy<IServiceProvider> serviceProvider) : AbstractCacheService(cacheProvider, serviceProvider), ICachePreheatable
{
    public override async Task PreheatAsync() => await Task.CompletedTask;
}
