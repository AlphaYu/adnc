using Adnc.Infra.Redis.Caching.Core.Preheater;

namespace Adnc.Demo.Ord.Application.Cahce;

public class CacheService(Lazy<ICacheProvider> cacheProvider, Lazy<IServiceProvider> serviceProvider) : AbstractCacheService(cacheProvider, serviceProvider), ICachePreheatable
{
    public override async Task PreheatAsync() => await Task.CompletedTask;
}
