using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Adnc.Infra.Core.Interceptor;

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
        private readonly IEasyCachingKeyGenerator _keyGenerator;

        /// <summary>
        /// The redis cache provider.
        /// </summary>
        private readonly IRedisDistributedCache _cacheProvider;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EasyCaching.Interceptor.Castle.EasyCachingInterceptor"/> class.
        /// </summary>
        /// <param name="cacheProvider">Cache provider .</param>
        /// <param name="keyGenerator">Key generator.</param>
        /// <param name="options">Options.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="hybridCachingProvider">Hybrid caching provider.</param>
        public CachingInterceptor(
            IRedisDistributedCache cacheProvider
            , IEasyCachingKeyGenerator keyGenerator
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
            //Process any early evictions 
            ProcessEvict(invocation, true);

            //Process any cache interceptor 
            ProceedAble(invocation);

            // Process any put requests
            ProcessPut(invocation);

            // Process any late evictions
            ProcessEvict(invocation, false);
        }
        private object[] GetMethodAttributes(MethodInfo mi)
        {
            return MethodAttributes.GetOrAdd(mi, mi.GetCustomAttributes(true));
        }

        /// <summary>
        /// Proceeds the able.
        /// </summary>
        /// <param name="invocation">Invocation.</param>
        private void ProceedAble(IInvocation invocation)
        {
            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;

            if (GetMethodAttributes(serviceMethod).FirstOrDefault(x => typeof(CachingAbleAttribute).IsAssignableFrom(x.GetType())) is CachingAbleAttribute attribute)
            {
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
            else
            {
                // Invoke the method if we don't have EasyCachingAbleAttribute
                invocation.Proceed();
            }
        }

        /// <summary>
        /// Processes the put.
        /// </summary>
        /// <param name="invocation">Invocation.</param>
        private void ProcessPut(IInvocation invocation)
        {
            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;

            if (GetMethodAttributes(serviceMethod).FirstOrDefault(x => typeof(CachingPutAttribute).IsAssignableFrom(x.GetType())) is CachingPutAttribute attribute && invocation.ReturnValue != null)
            {
                var cacheKey = string.IsNullOrEmpty(attribute.CacheKey)
                                     ? _keyGenerator.GetCacheKey(serviceMethod, invocation.Arguments, attribute.CacheKeyPrefix)
                                     : attribute.CacheKey
                                     ;
                try
                {
                    var returnValue = serviceMethod.IsReturnTask()
                           ? invocation.UnwrapAsyncReturnValue().Result
                           : invocation.ReturnValue;

                    _cacheProvider.Set(cacheKey, returnValue, TimeSpan.FromSeconds(attribute.Expiration));
                }
                catch (Exception ex)
                {
                    if (!attribute.IsHighAvailability) throw;
                    else _logger?.LogError(new EventId(), ex, $"Cache provider set error.");
                }
            }
        }

        /// <summary>
        /// Processes the evict.
        /// </summary>
        /// <param name="invocation">Invocation.</param>
        /// <param name="isBefore">If set to <c>true</c> is before.</param>
        private void ProcessEvict(IInvocation invocation, bool isBefore)
        {
            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;

            if (GetMethodAttributes(serviceMethod).FirstOrDefault(x => typeof(CachingEvictAttribute).IsAssignableFrom(x.GetType())) is CachingEvictAttribute attribute && attribute.IsBefore == isBefore)
            {
                try
                {
                    var cacheKey = attribute.CacheKey;
                    var cacheKeys = attribute.CacheKeys;
                    var cacheKeyPrefix = _keyGenerator.GetCacheKeyPrefix(serviceMethod, attribute.CacheKeyPrefix);

                    if (!string.IsNullOrEmpty(cacheKey))
                        _cacheProvider.Remove(cacheKey);
                    else if (cacheKeys?.Length > 0)
                        _cacheProvider.RemoveAll(cacheKeys);
                    else
                    {
                        //If is all , clear all cached items which cachekey start with the prefix.
                        if (attribute.IsAll)
                            _cacheProvider.RemoveByPrefix(cacheKeyPrefix);
                        else
                        {
                            cacheKey = _keyGenerator.GetCacheKey(serviceMethod, invocation.Arguments, attribute.CacheKeyPrefix);
                            _cacheProvider.Remove(cacheKey);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!attribute.IsHighAvailability) throw;
                    else _logger?.LogError(new EventId(), ex, $"Cache provider remove error.");
                }
            }
        }
    }
}
