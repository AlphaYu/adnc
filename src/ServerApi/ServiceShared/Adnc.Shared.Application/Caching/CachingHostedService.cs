namespace Adnc.Shared.Application.Caching;

public class CachingHostedService : BackgroundService
{
    private readonly ILogger<CachingHostedService> _logger;
    private readonly ICacheProvider _cacheProvider;
    private readonly ICachePreheatable _cachePreheatService;

    public CachingHostedService(
        ILogger<CachingHostedService> logger,
       ICacheProvider cacheProvider,
       ICachePreheatable cachePreheatService)
    {
        _logger = logger;
        _cacheProvider = cacheProvider;
        _cachePreheatService = cachePreheatService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //preheating caches
        await _cachePreheatService.PreheatAsync();

        //cofirming removed caches
        _ = Task.Factory.StartNew(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!LocalVariables.Instance.Queue.TryDequeue(out LocalVariables.Model? model)
                    || model.CacheKeys.IsNullOrEmpty()
                    || DateTime.Now > model.ExpireDt)
                {
                    await Task.Delay(_cacheProvider.CacheOptions.Value.LockMs, stoppingToken);
                    continue;
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (DateTime.Now > model.ExpireDt) break;

                    try
                    {
                        await _cacheProvider.RemoveAllAsync(model.CacheKeys);
                        break;
                    }
                    catch (Exception ex)
                    {
                        var message = $"{nameof(ExecuteAsync)}:{string.Join(",", model.CacheKeys)}";
                        _logger.LogError(ex, message);
                        await Task.Delay(_cacheProvider.CacheOptions.Value.LockMs, stoppingToken);
                    }
                }
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    }
}