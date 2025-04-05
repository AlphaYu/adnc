using Castle.DynamicProxy;

namespace Adnc.Infra.Redis.Caching.Core.Interceptor.Castle;

/// <summary>
/// caching async interceptor
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="T:Adnc.Infa.Caching.CachingAsyncInterceptor"/> class.
/// </remarks>
/// <param name="cacheProvider">Cache provider .</param>
/// <param name="keyGenerator">Key generator.</param>
/// <param name="logger">Logger.</param>
public sealed class CachingAsyncInterceptor(ICacheProvider cacheProvider, ICachingKeyGenerator keyGenerator, ILogger<CachingAsyncInterceptor>? logger = null) : IAsyncInterceptor
{
    /// <summary>
    /// 同步拦截器
    /// </summary>
    /// <param name="invocation"></param>
    public void InterceptSynchronous(IInvocation invocation)
    {
        var attribute = GetAttribute(invocation);
        if (attribute == null)
        {
            invocation.Proceed();
        }
        else
        {
            InternalInterceptSynchronous(invocation, attribute);
        }
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
             ? keyGenerator.GetCacheKey(methodInfo, invocation.Arguments, attribute.CacheKeyPrefix)
             : attribute.CacheKey
             ;

            try
            {
                var cacheValue = cacheProvider.GetAsync(cacheKey, methodInfo.ReturnType).GetAwaiter().GetResult();
                if (cacheKey != null)
                {
                    invocation.ReturnValue = cacheValue;
                }
                else
                {
                    invocation.Proceed();
                    if (!string.IsNullOrWhiteSpace(cacheKey) && invocation.ReturnValue != null)
                    {
                        cacheProvider.Set(cacheKey, invocation.ReturnValue, TimeSpan.FromSeconds(ableAttribute.Expiration));
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
                    logger?.LogError(new EventId(), ex, $"Cache provider get error.");
                }
            }
            return;
        }

        if (attribute is CachingEvictAttribute evictAttribute)
        {
            var (cacheKeys, expireDt) = ProcessEvictBefore(invocation, evictAttribute);

            var cancelTokenSource = new CancellationTokenSource();
            var timeoutPolicy = Policy.Timeout(cacheProvider.CacheOptions.Value.PollyTimeoutSeconds, Polly.Timeout.TimeoutStrategy.Optimistic);
            try
            {
                timeoutPolicy.Execute((cancellToken) =>
                {
                    invocation.Proceed();
                    cancellToken.ThrowIfCancellationRequested();
                }, cancelTokenSource.Token);

                cacheProvider.RemoveAll(cacheKeys);
            }
            catch (Exception ex)
            {
                LocalVariables.Instance.Queue.Enqueue(new LocalVariables.Model(cacheKeys, expireDt));

                if (!attribute.IsHighAvailability)
                {
                    throw;
                }
                else
                {
                    logger?.LogError(new EventId(), ex, $"Cache provider remove error.");
                }
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
            var timeoutPolicy = Policy.TimeoutAsync(cacheProvider.CacheOptions.Value.PollyTimeoutSeconds, Polly.Timeout.TimeoutStrategy.Optimistic);
            try
            {
                await timeoutPolicy.ExecuteAsync(async (cancellToken) =>
                {
                    invocation.Proceed();
                    var task = (Task)invocation.ReturnValue;
                    await task;
                    cancellToken.ThrowIfCancellationRequested();
                }, cancelTokenSource.Token);

                cacheProvider.RemoveAll(cacheKeys);
            }
            catch (Exception ex)
            {
                LocalVariables.Instance.Queue.Enqueue(new LocalVariables.Model(cacheKeys, expireDt));

                if (!attribute.IsHighAvailability)
                {
                    throw;
                }
                else
                {
                    logger?.LogError(new EventId(), ex, $"Cache provider remove error.");
                }
            }
        }
    }

    private static async Task InternalInterceptAsynchronousWithOutCaching(IInvocation invocation)
    {
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task;
    }

    private async Task<TResult?> InternalInterceptAsynchronous<TResult>(IInvocation invocation, CachingInterceptorAttribute attribute)
    {
        TResult? result = default;
        var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;

        if (attribute is CachingAbleAttribute ableAttribute)
        {
            var cacheKey = string.IsNullOrEmpty(attribute.CacheKey)
                 ? keyGenerator.GetCacheKey(methodInfo, invocation.Arguments, attribute.CacheKeyPrefix)
                 : attribute.CacheKey
                 ;
            try
            {
                var cacheValue = cacheProvider.Get<TResult>(cacheKey);
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
                        cacheProvider.Set(cacheKey, dbValue, TimeSpan.FromSeconds(ableAttribute.Expiration));
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
                    logger?.LogError(new EventId(), ex, $"Cache provider get error.");
                }
            }
            return result;
        }

        if (attribute is CachingEvictAttribute evictAttribute)
        {
            var (cacheKeys, expireDt) = ProcessEvictBefore(invocation, evictAttribute);
            var cancelTokenSource = new CancellationTokenSource();
            var timeoutPolicy = Policy.TimeoutAsync<TResult>(cacheProvider.CacheOptions.Value.PollyTimeoutSeconds, Polly.Timeout.TimeoutStrategy.Optimistic);
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

                cacheProvider.RemoveAll(cacheKeys);
            }
            catch (Exception ex)
            {
                LocalVariables.Instance.Queue.Enqueue(new LocalVariables.Model(cacheKeys, expireDt));

                if (!attribute.IsHighAvailability)
                {
                    throw;
                }
                else
                {
                    logger?.LogError(new EventId(), ex, $"Cache provider remove error.");
                }
            }

            return result;
        }

        return result;
    }

    private static async Task<TResult?> InternalInterceptAsynchronousWithOutCaching<TResult>(IInvocation invocation)
    {
        invocation.Proceed();
        var task = (Task<TResult>)invocation.ReturnValue;
        var result = await task;
        return result;
    }

    private static CachingInterceptorAttribute? GetAttribute(IInvocation invocation)
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
            var cacheKeys = keyGenerator.GetCacheKeys(serviceMethod, invocation.Arguments, attribute.CacheKeyPrefix);
            needRemovedKeys.UnionWith(cacheKeys);
        }

        var keyExpireSeconds = cacheProvider.CacheOptions.Value.PollyTimeoutSeconds + 1;
        cacheProvider.KeyExpireAsync(needRemovedKeys, keyExpireSeconds).GetAwaiter().GetResult();

        return (needRemovedKeys.ToList(), DateTime.Now.AddSeconds(keyExpireSeconds));
    }
}
