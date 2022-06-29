namespace Adnc.Shared.Application.Caching;

public class CachingHostedService : BackgroundService
{
    private readonly ILogger<CachingHostedService> _logger;
    private readonly ICacheProvider _cacheProvider;
    private readonly ICachePreheatable _cachePreheatService;

    public CachingHostedService(ILogger<CachingHostedService> logger
       , ICacheProvider cacheProvider
       , ICachePreheatable cachePreheatService)
    {
        _logger = logger;
        _cacheProvider = cacheProvider;
        _cachePreheatService = cachePreheatService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        #region Init Caches

        await _cachePreheatService.PreheatAsync();

        #endregion Init Caches

        #region Confirm Caches Removed

        while (!stoppingToken.IsCancellationRequested)
        {
            if (!LocalVariables.Instance.Queue.TryDequeue(out LocalVariables.Model model)
                || model.CacheKeys?.Any() == false
                || DateTime.Now > model.ExpireDt)
            {
                await Task.Delay(_cacheProvider.CacheOptions.Value.LockMs, stoppingToken);
                continue;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (DateTime.Now > model.ExpireDt) break;

                    await _cacheProvider.RemoveAllAsync(model.CacheKeys);

                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    await Task.Delay(_cacheProvider.CacheOptions.Value.LockMs, stoppingToken);
                }
            }
        }

        #endregion Confirm Caches Removed
    }
}