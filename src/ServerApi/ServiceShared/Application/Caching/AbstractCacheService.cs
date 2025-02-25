namespace Adnc.Shared.Application.Caching;

public abstract class AbstractCacheService : ICachePreheatable
{
    protected virtual Lazy<ICacheProvider> CacheProvider { get; private set; }
    protected virtual Lazy<IServiceProvider> ServiceProvider { get; private set; }
    protected virtual Lazy<IObjectMapper> Mapper { get; private set; }

    protected AbstractCacheService(Lazy<ICacheProvider> cacheProvider, Lazy<IServiceProvider> serviceProvider)
    {
        CacheProvider = cacheProvider;
        ServiceProvider = serviceProvider;
        Mapper = ServiceProvider.Value.GetRequiredService<Lazy<IObjectMapper>>();
    }

    public abstract Task PreheatAsync();

    public virtual string ConcatCacheKey(params object[] items)
    {
        if (items.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(items));

        return string.Join(GeneralConsts.LinkChar, items);
    }

    public virtual async Task RemoveCachesAsync(Func<CancellationToken, Task> dataOperater, params string[] cacheKeys)
    {
        var pollyTimeoutSeconds = CacheProvider.Value.CacheOptions.Value.PollyTimeoutSeconds;
        var keyExpireSeconds = pollyTimeoutSeconds + 1;

        await CacheProvider.Value.KeyExpireAsync(cacheKeys, keyExpireSeconds);

        var expireDt = DateTime.Now.AddSeconds(keyExpireSeconds);
        var cancelTokenSource = new CancellationTokenSource();
        var timeoutPolicy = Policy.TimeoutAsync(pollyTimeoutSeconds, Polly.Timeout.TimeoutStrategy.Optimistic);
        await timeoutPolicy.ExecuteAsync(async (cancellToken) =>
        {
            await dataOperater(cancellToken);
            cancellToken.ThrowIfCancellationRequested();
        }, cancelTokenSource.Token);

        try
        {
            await CacheProvider.Value.RemoveAllAsync(cacheKeys);
        }
        catch (Exception ex)
        {
            LocalVariables.Instance.Queue.Enqueue(new LocalVariables.Model(cacheKeys, expireDt));
            var logger = ServiceProvider.Value.GetRequiredService<Lazy<ILogger<AbstractCacheService>>>();
            logger.Value.LogError(ex, ex.Message);
        }
    }
}