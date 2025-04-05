namespace Adnc.Infra.Redis.Caching.Core.Preheater;

public class CachingHostedService(ILogger<CachingHostedService> logger, ICacheProvider cacheProvider, ICachePreheatable cachePreheatService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //preheating caches
        await cachePreheatService.PreheatAsync();

        //cofirming removed caches
        _ = Task.Factory.StartNew(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!LocalVariables.Instance.Queue.TryDequeue(out var model)
                    || model is null
                    || model.CacheKeys.IsNullOrEmpty()
                    || DateTime.Now > model.ExpireDt)
                {
                    await Task.Delay(cacheProvider.CacheOptions.Value.LockMs, stoppingToken);
                    continue;
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (DateTime.Now > model.ExpireDt)
                    {
                        break;
                    }

                    try
                    {
                        await cacheProvider.RemoveAllAsync(model.CacheKeys);
                        break;
                    }
                    catch (Exception ex)
                    {
                        var message = $"{nameof(ExecuteAsync)}:{string.Join(",", model.CacheKeys)}";
                        logger.LogError(ex, "remove cache kes error ：{message}", message);
                        await Task.Delay(cacheProvider.CacheOptions.Value.LockMs, stoppingToken);
                    }
                }
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    }
}
