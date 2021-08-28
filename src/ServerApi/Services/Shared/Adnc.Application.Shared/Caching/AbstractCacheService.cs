using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Core;
using Adnc.Infra.Mapper;
using Adnc.Shared.Consts.Caching.Com;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Application.Shared.Caching
{
    public abstract class AbstractCacheService : ICacheService
    {
        private readonly Lazy<ICacheProvider> _cache;

        protected AbstractCacheService(Lazy<ICacheProvider> cache)
            => _cache = cache;

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
            var pollyTimeoutSeconds = _cache.Value.CacheOptions.PollyTimeoutSeconds;
            var keyExpireSeconds = pollyTimeoutSeconds + 1;

            await _cache.Value.KeyExpireAsync(cacheKeys, keyExpireSeconds);

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
                await _cache.Value.RemoveAllAsync(cacheKeys);
            }
            catch (Exception ex)
            {
                LocalVariables.Instance.Queue.Enqueue(new LocalVariables.Model(cacheKeys, expireDt));
                throw new TimeoutException(ex.Message, ex);
            }
        }
    }
}