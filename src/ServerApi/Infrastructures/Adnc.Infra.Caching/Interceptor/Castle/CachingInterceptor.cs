using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Adnc.Infra.Core.Interceptor;
using Adnc.Infra.Caching.Core;
using System.Collections.Generic;

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
            //Process any early evictions 
            var preRemoveKey = ProcessEvictBefore(invocation);

            //Process any cache interceptor 
            ProceedAble(invocation);

            // Process any put requests
            ProcessPut(invocation);

            // Process any late evictions
            ProcessEvictAfter(invocation, preRemoveKey);
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
        /// <param name="invocation">IInvocation.</param>
        /// <returns></returns>
        private string ProcessEvictBefore(IInvocation invocation)
        {
            string preRemoveKey = string.Empty;
            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;

            if (GetMethodAttributes(serviceMethod).FirstOrDefault(x => typeof(CachingEvictAttribute).IsAssignableFrom(x.GetType())) is CachingEvictAttribute attribute)
            {
                var needRemovedKeys = new HashSet<string>();
                if (!string.IsNullOrEmpty(attribute.CacheKey))
                {
                    needRemovedKeys.Add(attribute.CacheKey);
                }
                
                if (attribute.CacheKeys?.Length > 0)
                {
                    needRemovedKeys.UnionWith(attribute.CacheKeys);
                }

                if(!string.IsNullOrWhiteSpace(attribute.CacheKeyPrefix))
                {
                    var cacheKeys = _keyGenerator.GetCacheKeys(serviceMethod, invocation.Arguments, attribute.CacheKeyPrefix);
                    needRemovedKeys.UnionWith(cacheKeys);
                }
                //if (attribute.IsAll)
                //{
                //    preRemoveKey = $"{CachingConstValue.PreRemoveAllKeyPrefix}{LinkChar}{ attribute.CacheKeyPrefix.GetHashCode()}";
                //    needRemovedKeys = new string[] { attribute.CacheKeyPrefix, preRemoveKey };
                //}
                //else
                //{
                //    var cacheKey = _keyGenerator.GetCacheKey(serviceMethod, invocation.Arguments, attribute.CacheKeyPrefix);
                //    preRemoveKey = $"{CachingConstValue.PreRemoveKey}{LinkChar}{cacheKey.GetHashCode()}";
                //    needRemovedKeys = new string[] { cacheKey, preRemoveKey };
                //}

                preRemoveKey = $"{CachingConstValue.PreRemoveKey}{LinkChar}{string.Join(",", needRemovedKeys).GetHashCode()}";
                needRemovedKeys.Add(preRemoveKey);
                _cacheProvider.Set(preRemoveKey, needRemovedKeys.ToArray(), TimeSpan.FromSeconds(60 * 60 * 24));
            }
            return preRemoveKey;
        }

        /// <summary>
        /// Processes the evict.
        /// </summary>
        /// <param name="invocation">Invocation.</param>
        /// <param name="preRemoveKey">preRemoveKey</param>
        private void ProcessEvictAfter(IInvocation invocation, string preRemoveKey)
        {
            if (string.IsNullOrWhiteSpace(preRemoveKey))
                return;

            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;
            if (GetMethodAttributes(serviceMethod).FirstOrDefault(x => typeof(CachingEvictAttribute).IsAssignableFrom(x.GetType())) is CachingEvictAttribute attribute)
            {
                try
                {
                    var needRemoveCacheKeys = _cacheProvider.Get<string[]>(preRemoveKey).Value;
                    _cacheProvider.RemoveAll(needRemoveCacheKeys);
                    //if (preRemoveKey.StartsWith(CachingConstValue.PreRemoveAllKeyPrefix))
                    //{
                    //    _cacheProvider.RemoveByPrefix(needRemoveCacheKeys[0]);
                    //    _cacheProvider.Remove(needRemoveCacheKeys[1]);
                    //}
                    //else
                    //{
                    //    _cacheProvider.RemoveAll(needRemoveCacheKeys);
                    //}
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
