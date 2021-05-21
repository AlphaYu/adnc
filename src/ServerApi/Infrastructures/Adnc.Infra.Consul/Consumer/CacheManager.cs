using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace Adnc.Infra.Consul.Consumer
{
    public class CacheManager
    {
        //volatile
        private static readonly IMemoryCache _memoryCache;

        private static readonly object _lockObject = new object();

        private CacheManager()
        {
        }

        static CacheManager()
        {
            if (_memoryCache == null)
            {
                lock (_lockObject)
                {
                    if (_memoryCache == null)
                    {
                        _memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
                    }
                }
            }
        }

        public static object Get(object key)
        {
            return _memoryCache.Get(key);
        }

        public static TItem Get<TItem>(object key)
        {
            return (TItem)_memoryCache.Get(key);
        }

        public static bool TryGetValue<TItem>(object key, out TItem value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        public static TItem Set<TItem>(object key, TItem value)
        {
            return _memoryCache.Set(key, value);
        }

        public static TItem Set<TItem>(object key, TItem value, DateTimeOffset absoluteExpiration)
        {
            return _memoryCache.Set(key, value, absoluteExpiration);
        }

        public static TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
        {
            return _memoryCache.Set(key, value, absoluteExpirationRelativeToNow);
        }

        public static TItem Set<TItem>(object key, TItem value, IChangeToken expirationToken)
        {
            return _memoryCache.Set(key, value, expirationToken);
        }

        public static TItem Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options)
        {
            return _memoryCache.Set(key, value, options);
        }

        public static TItem GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory)
        {
            return _memoryCache.GetOrCreate(key, factory);
        }

        public static async Task<TItem> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory)
        {
            return await _memoryCache.GetOrCreateAsync(key, factory);
        }

        public static void Remove(object key)
        {
            _memoryCache.Remove(key);
        }
    }
}