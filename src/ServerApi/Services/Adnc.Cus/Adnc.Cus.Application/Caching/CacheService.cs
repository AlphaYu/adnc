namespace Adnc.Cus.Application.Services.Caching;

public sealed class CacheService : AbstractCacheService, ICachePreheatable
{
    public CacheService(
        Lazy<ICacheProvider> cacheProvider,
        Lazy<IServiceProvider> serviceProvider)
        : base(cacheProvider, serviceProvider)
    {
    }

    public override async Task PreheatAsync() => await Task.CompletedTask;
}