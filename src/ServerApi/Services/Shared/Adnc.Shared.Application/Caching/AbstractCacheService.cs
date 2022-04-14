namespace Adnc.Shared.Application.Caching;

public abstract class AbstractCacheService : ICachePreheatable
{
    private readonly Lazy<ICacheProvider> _cacheProvider;

    protected AbstractCacheService(Lazy<ICacheProvider> cacheProvider) => _cacheProvider = cacheProvider;

    public IObjectMapper Mapper { get; set; }

    public abstract Task PreheatAsync();

    public virtual string ConcatCacheKey(params object[] items)
    {
        if (items == null || items.Length == 0)
            throw new ArgumentNullException(nameof(items));

        return string.Join(CachingConsts.LinkChar, items);
    }

    public virtual async Task RemoveCachesAsync(Func<CancellationToken, Task> dataOperater, params string[] cacheKeys)
    {
        var pollyTimeoutSeconds = _cacheProvider.Value.CacheOptions.PollyTimeoutSeconds;
        var keyExpireSeconds = pollyTimeoutSeconds + 1;

        await _cacheProvider.Value.KeyExpireAsync(cacheKeys, keyExpireSeconds);

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
            await _cacheProvider.Value.RemoveAllAsync(cacheKeys);
        }
        catch (Exception ex)
        {
            LocalVariables.Instance.Queue.Enqueue(new LocalVariables.Model(cacheKeys, expireDt));
            throw new TimeoutException(ex.Message, ex);
        }
    }
}