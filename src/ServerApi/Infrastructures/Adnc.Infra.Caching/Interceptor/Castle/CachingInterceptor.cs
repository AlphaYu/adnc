using Adnc.Infra.Caching.Core;
using Adnc.Infra.Core.Interceptor;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Infra.Caching.Interceptor.Castle
{
    /// <summary>
    /// Easycaching interceptor.
    /// </summary>
    public class CachingInterceptor : IInterceptor
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
        public ILogger<CachingInterceptor> _logger;

        /// <summary>
        /// The typeof task result method.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, MethodInfo>
                    TypeofTaskResultMethod = new ConcurrentDictionary<Type, MethodInfo>();

        /// <summary>
        /// The typeof task result method.
        /// </summary>
        private static readonly ConcurrentDictionary<MethodInfo, object[]>
                    MethodAttributes = new ConcurrentDictionary<MethodInfo, object[]>();

        private const string LinkChar = ":";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyCaching.Interceptor.Castle.EasyCachingInterceptor"/> class.
        /// </summary>
        /// <param name="cacheProvider">Cache provider .</param>
        /// <param name="keyGenerator">Key generator.</param>
        /// <param name="options">Options.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="hybridCachingProvider">Hybrid caching provider.</param>
        public CachingInterceptor(
            ICacheProvider cacheProvider
            , ICachingKeyGenerator keyGenerator
            , ILogger<CachingInterceptor> logger = null)
        {
            _cacheProvider = cacheProvider;
            _keyGenerator = keyGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Intercept the specified invocation.
        /// </summary>
        /// <returns>The intercept.</returns>
        /// <param name="invocation">Invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;
            var attribute = GetMethodAttributes(serviceMethod).FirstOrDefault(x => typeof(CachingInterceptorAttribute).IsAssignableFrom(x.GetType()));

            if (attribute == null)
            {
                invocation.Proceed();
                return;
            }

            //// Process any evictions
            if (attribute is CachingEvictAttribute evictAttribute)
            {
                var removedKeys = ProcessEvictBefore(invocation, evictAttribute);
                if (removedKeys?.Any() == true)
                    ProcessEvictAfter(invocation, evictAttribute, removedKeys);
                return;
            }

            //Process any cache interceptor
            if (attribute is CachingAbleAttribute ableAttribute)
            {
                ProceedAble(invocation, ableAttribute);
                return;
            }
        }

        private object[] GetMethodAttributes(MethodInfo mi)
        {
            return MethodAttributes.GetOrAdd(mi, mi.GetCustomAttributes(true));
        }

        /// <summary>
        /// Proceeds the able.
        /// </summary>
        /// <param name="invocation">Invocation.</param>
        private void ProceedAble(IInvocation invocation, CachingAbleAttribute attribute)
        {
            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;

            var returnType = serviceMethod.IsReturnTask()
                    ? serviceMethod.ReturnType.GetGenericArguments().First()
                    : serviceMethod.ReturnType;

            var cacheKey = string.IsNullOrEmpty(attribute.CacheKey)
                                 ? _keyGenerator.GetCacheKey(serviceMethod, invocation.Arguments, attribute.CacheKeyPrefix)
                                 : attribute.CacheKey
                                 ;

            object cacheValue = null;
            var isAvailable = true;
            try
            {
                cacheValue = _cacheProvider.GetAsync(cacheKey, returnType).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                if (!attribute.IsHighAvailability)
                {
                    throw;
                }
                else
                {
                    isAvailable = false;
                    _logger?.LogError(new EventId(), ex, $"Cache provider get error.");
                }
            }

            if (cacheValue != null)
            {
                if (serviceMethod.IsReturnTask())
                {
                    invocation.ReturnValue =
                        TypeofTaskResultMethod.GetOrAdd(returnType, t => typeof(Task).GetMethods().First(p => p.Name == "FromResult" && p.ContainsGenericParameters).MakeGenericMethod(returnType)).Invoke(null, new object[] { cacheValue });
                }
                else
                {
                    invocation.ReturnValue = cacheValue;
                }
            }
            else
            {
                // Invoke the method if we don't have a cache hit
                invocation.Proceed();

                if (!string.IsNullOrWhiteSpace(cacheKey) && invocation.ReturnValue != null && isAvailable)
                {
                    // get the result
                    var returnValue = serviceMethod.IsReturnTask()
                       ? invocation.UnwrapAsyncReturnValue().Result
                       : invocation.ReturnValue;

                    // should we do something when method return null?
                    // 1. cached a null value for a short time
                    // 2. do nothing
                    if (returnValue != null)
                    {
                        _cacheProvider.Set(cacheKey, returnValue, TimeSpan.FromSeconds(attribute.Expiration));
                    }
                }
            }
        }

        /// <summary>
        /// Processes the put.
        /// </summary>
        /// <param name="invocation">Invocation.</param>
        //private void ProcessPut(IInvocation invocation, CachingPutAttribute attribute)
        //{
        //    var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;

        //    var cacheKey = string.IsNullOrEmpty(attribute.CacheKey)
        //                         ? _keyGenerator.GetCacheKey(serviceMethod, invocation.Arguments, attribute.CacheKeyPrefix)
        //                         : attribute.CacheKey
        //                         ;
        //    try
        //    {
        //        var returnValue = serviceMethod.IsReturnTask()
        //               ? invocation.UnwrapAsyncReturnValue().Result
        //               : invocation.ReturnValue;

        //        _cacheProvider.Set(cacheKey, returnValue, TimeSpan.FromSeconds(attribute.Expiration));
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!attribute.IsHighAvailability) throw;
        //        else _logger?.LogError(new EventId(), ex, $"Cache provider set error.");
        //    }
        //}

        /// <summary>
        /// Processes the evict.
        /// </summary>
        /// <param name="invocation">IInvocation.</param>
        /// <returns></returns>
        private List<string> ProcessEvictBefore(IInvocation invocation, CachingEvictAttribute attribute)
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
            return needRemovedKeys.ToList();
        }

        /// <summary>
        /// Processes the evict.
        /// </summary>
        /// <param name="invocation">Invocation.</param>
        /// <param name="preRemoveKey">preRemoveKey</param>
        private void ProcessEvictAfter(IInvocation invocation, CachingEvictAttribute attribute, IEnumerable<string> cacheKeys)
        {
            var pollyTimeoutSeconds = _cacheProvider.CacheOptions.PollyTimeoutSeconds;
            var keyExpireSeconds = pollyTimeoutSeconds + 1;

            _cacheProvider.KeyExpireAsync(cacheKeys, keyExpireSeconds).GetAwaiter().GetResult();

            var expireDt = DateTime.Now.AddSeconds(keyExpireSeconds);
            var cancelTokenSource = new CancellationTokenSource();
            var timeoutPolicy = Policy.Timeout(pollyTimeoutSeconds, Polly.Timeout.TimeoutStrategy.Optimistic);
            timeoutPolicy.Execute((cancellToken) =>
            {
                invocation.Proceed();
                cancellToken.ThrowIfCancellationRequested();
            }, cancelTokenSource.Token);

            try
            {
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
}