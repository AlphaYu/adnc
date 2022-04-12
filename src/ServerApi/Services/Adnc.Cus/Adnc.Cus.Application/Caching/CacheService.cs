namespace Adnc.Cus.Application.Services.Caching;

public class CacheService : AbstractCacheService,ICachePreheatable
{
    private readonly Lazy<ICacheProvider> _cacheProvider;

    public CacheService(Lazy<ICacheProvider> cacheProvider)
        : base(cacheProvider)
        => _cacheProvider = cacheProvider;

    public override async Task PreheatAsync()
    {
        // TODO
        await Task.CompletedTask;
    }
}