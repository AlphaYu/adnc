namespace Adnc.Application.Shared.Caching;

public class CachingHostedService : BackgroundService
{
    private readonly ILogger<CachingHostedService> _logger;
    private readonly ICacheProvider _cache;
    private readonly ICacheService _cacheService;

    public CachingHostedService(ILogger<CachingHostedService> logger
       , ICacheProvider cache
       , ICacheService cacheService)
    {
        _logger = logger;
        _cache = cache;
        _cacheService = cacheService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        #region Init Caches

        await _cacheService.PreheatAsync();

        #endregion Init Caches

        #region Confirm Caches Removed

        while (!stoppingToken.IsCancellationRequested)
        {
            if (!LocalVariables.Instance.Queue.TryDequeue(out LocalVariables.Model model)
                || model.CacheKeys?.Any() == false
                || DateTime.Now > model.ExpireDt)
            {
                await Task.Delay(_cache.CacheOptions.LockMs, stoppingToken);
                continue;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (DateTime.Now > model.ExpireDt) break;

                    await _cache.RemoveAllAsync(model.CacheKeys);

                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    await Task.Delay(_cache.CacheOptions.LockMs, stoppingToken);
                }
            }
        }

        #endregion Confirm Caches Removed
    }
}