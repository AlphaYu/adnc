using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Polly;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Core;
using Adnc.Infra.Mapper;
using Adnc.Application.Shared.Consts;

namespace Adnc.Application.Shared.Caching
{
    public interface ICacheService { }

    public abstract class AbstractCacheService : ICacheService
    {
        private readonly Lazy<ICacheProvider> _cache;
        private readonly Lazy<IRedisProvider> _redisProvider;

        public AbstractCacheService(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider)
        {
            _cache = cache;
            _redisProvider = redisProvider;
        }

        internal IRedisProvider RedisProvider => _redisProvider.Value;

        public IObjectMapper Mapper { get; set; }

        public string GetPreRemoveKey(IEnumerable<string> cacheKeys)
        {
            var hashCode = string.Join(",", cacheKeys).GetHashCode();
            return $"{CachingConstValue.PreRemoveKey}{SharedCachingConsts.LinkChar}{hashCode}";
        }

        public string ConcatCacheKey(params object[] items)
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

        public async Task RemoveCachesAsync(Func<CancellationToken, Task> dataOperater, params string[] cacheKeys)
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
