namespace Adnc.Whse.Application.Services.Caching;

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