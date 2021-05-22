using Adnc.Application.Shared.Consts;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Core;
using Adnc.Infra.Mapper;
using Polly;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Application.Shared.Caching
{
    public interface ICacheService
    {
        /// <summary>
        /// 预热缓存
        /// </summary>
        /// <returns></returns>
        Task PreheatAsync();
    }

    public abstract class AbstractCacheService : ICacheService
    {
        private readonly Lazy<ICacheProvider> _cache;
        private readonly Lazy<IRedisProvider> _redisProvider;
        private readonly Lazy<IDistributedLocker> _distributedLocker;

        public AbstractCacheService(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker)
        {
            _cache = cache;
            _redisProvider = redisProvider;
            _distributedLocker = distributedLocker;
        }

        public IObjectMapper Mapper { get; set; }

        public abstract Task PreheatAsync();

        public virtual string ConcatCacheKey(params object[] items)
        {
            if (items == null || items.Length == 0)
                return string.Empty;

            var sbuilder = new StringBuilder();
            int index = 0;
            int total = items.Length;
            foreach (var item in items)
            {
                index++;
                sbuilder.Append(item);
                if (index != total)
                    sbuilder.Append(SharedCachingConsts.LinkChar);
            }
            return sbuilder.ToString();
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
                throw new Exception(ex.Message, ex);
            }
        }
    }
}