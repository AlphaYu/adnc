using Adnc.Infra.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adnc.Application.Shared.Caching
{
    public interface IBloomFilter
    {
        string Name { get; }
        double ErrorRate { get; }
        int Capacity { get; }

        /// <summary>
        /// 初始化布隆过滤器
        /// </summary>
        /// <returns></returns>
        Task InitAsync();

        Task<bool> AddAsync(string value);

        Task<bool[]> AddAsync(IEnumerable<string> values);

        Task<bool> ExistsAsync(string value);

        Task<bool[]> ExistsAsync(IEnumerable<string> values);
    }

    public abstract class AbstractBloomFilter : IBloomFilter
    {
        private readonly Lazy<ICacheProvider> _cache;
        private readonly Lazy<IRedisProvider> _redisProvider;
        private readonly Lazy<IDistributedLocker> _distributedLocker;

        public AbstractBloomFilter(Lazy<ICacheProvider> cache
            , Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker)
        {
            _cache = cache;
            _redisProvider = redisProvider;
            _distributedLocker = distributedLocker;
        }

        public abstract string Name { get; }

        public abstract double ErrorRate { get; }

        public abstract int Capacity { get; }

        public virtual async Task<bool> AddAsync(string value) => await _redisProvider.Value.BloomAddAsync(this.Name, value);

        public virtual async Task<bool[]> AddAsync(IEnumerable<string> values) => await _redisProvider.Value.BloomAddAsync(this.Name, values);

        public virtual async Task<bool> ExistsAsync(string value) => await _redisProvider.Value.BloomExistsAsync(this.Name, value);

        public virtual async Task<bool[]> ExistsAsync(IEnumerable<string> values) => await _redisProvider.Value.BloomExistsAsync(this.Name, values);

        public abstract Task InitAsync();

        protected async Task InitAsync(Func<Task<IEnumerable<string>>> InitValues)
        {
            if (await _cache.Value.ExistsAsync(this.Name))
                return;

            var flag = await _distributedLocker.Value.LockAsync(this.Name);
            if (!flag.Success)
            {
                await Task.Delay(_cache.Value.CacheOptions.SleepMs);
                await InitAsync(InitValues);
            }

            try
            {
                var values = await InitValues();
                await _redisProvider.Value.BloomReserveAsync(this.Name, this.ErrorRate, this.Capacity);
                if (values?.Any() == true)
                    await _redisProvider.Value.BloomAddAsync(this.Name, values);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                await _distributedLocker.Value.SafedUnLockAsync(this.Name, flag.LockValue);
            }
        }
    }
}