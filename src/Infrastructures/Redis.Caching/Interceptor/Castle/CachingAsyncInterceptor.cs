using Adnc.Infra.Redis.Caching.Core;
using Adnc.Infra.Redis.Caching.Core.Interceptor;
using Castle.DynamicProxy;

namespace Adnc.Infra.Redis.Caching.Interceptor.Castle
{
    /// <summary>
    /// caching async interceptor
    /// </summary>
    public sealed class CachingAsyncInterceptor : IAsyncInterceptor
    {
        /// <summary>
        /// The key generator.
        /// </summary>
        private readonly ICachingKeyGenerator _keyGenerator;

        /// <summary>
        /// The redis cache provider.
        /// </summary>
        private readonly ICacheProvider _cacheProvider;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<CachingAsyncInterceptor>? _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Adnc.Infa.Caching.CachingAsyncInterceptor"/> class.
        /// </summary>
        /// <param name="cacheProvider">Cache provider .</param>
        /// <param name="keyGenerator">Key generator.</param>
        /// <param name="options">Options.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="hybridCachingProvider">Hybrid caching provider.</param>
        public CachingAsyncInterceptor(
            ICacheProvider cacheProvider
            , ICachingKeyGenerator keyGenerator
            , ILogger<CachingAsyncInterceptor>? logger = null)
        {
            _cacheProvider = cacheProvider;
            _keyGenerator = keyGenerator;
            _logger = logger;
        }

        /// <summary>
        /// 同步拦截器
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);
            if (attribute == null)
                invocation.Proceed();
            else
                InternalInterceptSynchronous(invocation, attribute);
        }

        /// <summary>
        /// 异步拦截器 无返回值
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);

            invocation.ReturnValue = attribute == null
                                     ? InternalInterceptAsynchronousWithOutCaching(invocation)
                                     : InternalInterceptAsynchronous(invocation, attribute)
                                     ;
        }

        /// <summary>
        /// 异步拦截器 有返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);

            invocation.ReturnValue = attribute == null
               ? InternalInterceptAsynchronousWithOutCaching<TResult>(invocation)
                : InternalInterceptAsynchronous<TResult>(invocation, attribute)
                ;
        }

        private void InternalInterceptSynchronous(IInvocation invocation, CachingInterceptorAttribute attribute)
        {
            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
            if (attribute is CachingAbleAttribute ableAttribute)
            {
                var cacheKey = string.IsNullOrEmpty(attribute.CacheKey)
                 ? _keyGenerator.GetCacheKey(methodInfo, invocation.Arguments, attribute.CacheKeyPrefix)
                 : attribute.CacheKey
                 ;

                try
                {
                    var cacheValue = _cacheProvider.GetAsync(cacheKey, methodInfo.ReturnType).GetAwaiter().GetResult();
                    if (cacheKey != null)
                    {
                        invocation.ReturnValue = cacheValue;
                    }
                    else
                    {
                        invocation.Proceed();
                        if (!string.IsNullOrWhiteSpace(cacheKey) && invocation.ReturnValue != null)
                        {
                            _cacheProvider.Set(cacheKey, invocation.ReturnValue, TimeSpan.FromSeconds(ableAttribute.Expiration));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!attribute.IsHighAvailability)
                    {
                        throw;
                    }
                    else
                    {
                        _logger?.LogError(new EventId(), ex, $"Cache provider get error.");
                    }
                }
                return;
            }

            if (attribute is CachingEvictAttribute evictAttribute)
            {
                var (cacheKeys, expireDt) = ProcessEvictBefore(invocation, evictAttribute);

                var cancelTokenSource = new CancellationTokenSource();
                var timeoutPolicy = Policy.Timeout(_cacheProvider.CacheOptions.Value.PollyTimeoutSeconds, Polly.Timeout.TimeoutStrategy.Optimistic);
                try
                {
                    timeoutPolicy.Execute((cancellToken) =>
                    {
                        invocation.Proceed();
                        cancellToken.ThrowIfCancellationRequested();
                    }, cancelTokenSource.Token);

                    _cacheProvider.RemoveAll(cacheKeys);
                }
                catch (Exception ex)
                {
                    LocalVariables.Instance.Queue.Enqueue(new LocalVariables.Model(cacheKeys, expireDt));

                    if (!attribute.IsHighAvailability) throw;
                    else _logger?.LogError(new EventId(), ex, $"Cache provider remove error.");
                }
            }
        }

        private async Task InternalInterceptAsynchronous(IInvocation invocation, CachingInterceptorAttribute attribute)
        {
            if (attribute is CachingAbleAttribute ableAttribute)
            {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
                return;
            }

            if (attribute is CachingEvictAttribute evictAttribute)
            {
                var (cacheKeys, expireDt) = ProcessEvictBefore(invocation, evictAttribute);
                var cancelTokenSource = new CancellationTokenSource();
                var timeoutPolicy = Policy.TimeoutAsync(_cacheProvider.CacheOptions.Value.PollyTimeoutSeconds, Polly.Timeout.TimeoutStrategy.Optimistic);
                try
                {
                    await timeoutPolicy.ExecuteAsync(async (cancellToken) =>
                    {
                        invocation.Proceed();
                        var task = (Task)invocation.ReturnValue;
                        await task;
                        cancellToken.ThrowIfCancellationRequested();
                    }, cancelTokenSource.Token);

                    _cacheProvider.RemoveAll(cacheKeys);
                }
                catch (Exception ex)
                {
                    LocalVariables.Instance.Queue.Enqueue(new LocalVariables.Model(cacheKeys, expireDt));

                    if (!attribute.IsHighAvailability) throw;
                    else _logger?.LogError(new EventId(), ex, $"Cache provider remove error.");
                }
            }
        }

        private async Task InternalInterceptAsynchronousWithOutCaching(IInvocation invocation)
        {
            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task;
        }

        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation, CachingInterceptorAttribute attribute)
        {
            TResult result = default;
            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;

            if (attribute is CachingAbleAttribute ableAttribute)
            {
                var cacheKey = string.IsNullOrEmpty(attribute.CacheKey)
                     ? _keyGenerator.GetCacheKey(methodInfo, invocation.Arguments, attribute.CacheKeyPrefix)
                     : attribute.CacheKey
                     ;
                try
                {
                    var cacheValue = _cacheProvider.Get<TResult>(cacheKey);
                    if (cacheValue.HasValue)
                    {
                        result = cacheValue.Value;
                    }
                    else
                    {
                        invocation.Proceed();
                        var task = (Task<TResult>)invocation.ReturnValue;
                        var dbValue = await task;

                        if (!string.IsNullOrWhiteSpace(cacheKey) && dbValue != null)
                        {
                            _cacheProvider.Set(cacheKey, dbValue, TimeSpan.FromSeconds(ableAttribute.Expiration));
                            result = dbValue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!attribute.IsHighAvailability)
                    {
                        throw;
                    }
                    else
                    {
                        _logger?.LogError(new EventId(), ex, $"Cache provider get error.");
                    }
                }
                return result;
            }

            if (attribute is CachingEvictAttribute evictAttribute)
            {
                var (cacheKeys, expireDt) = ProcessEvictBefore(invocation, evictAttribute);
                var cancelTokenSource = new CancellationTokenSource();
                var timeoutPolicy = Policy.TimeoutAsync<TResult>(_cacheProvider.CacheOptions.Value.PollyTimeoutSeconds, Polly.Timeout.TimeoutStrategy.Optimistic);
                try
                {
                    result = await timeoutPolicy.ExecuteAsync(async (cancellToken) =>
                    {
                        invocation.Proceed();
                        var task = (Task<TResult>)invocation.ReturnValue;
                        result = await task;
                        cancellToken.ThrowIfCancellationRequested();
                        return result;
                    }, cancelTokenSource.Token);

                    _cacheProvider.RemoveAll(cacheKeys);
                }
                catch (Exception ex)
                {
                    LocalVariables.Instance.Queue.Enqueue(new LocalVariables.Model(cacheKeys, expireDt));

                    if (!attribute.IsHighAvailability) throw;
                    else _logger?.LogError(new EventId(), ex, $"Cache provider remove error.");
                }

                return result;
            }

            return result;
        }

        private async Task<TResult> InternalInterceptAsynchronousWithOutCaching<TResult>(IInvocation invocation)
        {
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            TResult result = await task;
            return result;
        }

        private CachingInterceptorAttribute GetAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
            var attribute = methodInfo.GetCustomAttribute<CachingInterceptorAttribute>();
            return attribute;
        }

        private (List<string> cacheKeys, DateTime expireDt) ProcessEvictBefore(IInvocation invocation, CachingEvictAttribute attribute)
        {
            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;
            var needRemovedKeys = new HashSet<string>();

            if (!string.IsNullOrEmpty(attribute.CacheKey))
            {
                needRemovedKeys.Add(attribute.CacheKey);
            }

            if (attribute.CacheKeys?.Length > 0)
            {
                needRemovedKeys.UnionWith(attribute.CacheKeys);
            }

            if (!string.IsNullOrWhiteSpace(attribute.CacheKeyPrefix))
            {
                var cacheKeys = _keyGenerator.GetCacheKeys(serviceMethod, invocation.Arguments, attribute.CacheKeyPrefix);
                needRemovedKeys.UnionWith(cacheKeys);
            }

            var keyExpireSeconds = _cacheProvider.CacheOptions.Value.PollyTimeoutSeconds + 1;
            _cacheProvider.KeyExpireAsync(needRemovedKeys, keyExpireSeconds).GetAwaiter().GetResult();

            return (needRemovedKeys.ToList(), DateTime.Now.AddSeconds(keyExpireSeconds));
        }
    }
}