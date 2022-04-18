using Adnc.Infra.Caching;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Application.Shared.BloomFilter
{
    public abstract class AbstractBloomFilter : IBloomFilter
    {
        private readonly Lazy<IRedisProvider> _redisProvider;
        private readonly Lazy<IDistributedLocker> _distributedLocker;

        protected AbstractBloomFilter(Lazy<IRedisProvider> redisProvider
            , Lazy<IDistributedLocker> distributedLocker)
        {
            _redisProvider = redisProvider;
            _distributedLocker = distributedLocker;
        }

        public abstract string Name { get; }

        public abstract double ErrorRate { get; }

        public abstract int Capacity { get; }

        public virtual async Task<bool> AddAsync(string value)
        {
            var exists = await this.ExistsBloomFilterAsync();
            if (!exists)
                throw new ArgumentNullException(this.Name, $"call {nameof(InitAsync)} methos before");

            return await _redisProvider.Value.BloomAddAsync(this.Name, value);
        }

        public virtual async Task<bool[]> AddAsync(IEnumerable<string> values)
        {
            var exists = await this.ExistsBloomFilterAsync();
            if (!exists)
                throw new ArgumentNullException(this.Name, $"call {nameof(InitAsync)} methos before");

            return await _redisProvider.Value.BloomAddAsync(this.Name, values);
        }

        public virtual async Task<bool> ExistsAsync(string value)
            => await _redisProvider.Value.BloomExistsAsync(this.Name, value);

        public virtual async Task<bool[]> ExistsAsync(IEnumerable<string> values)
            => await _redisProvider.Value.BloomExistsAsync(this.Name, values);

        public abstract Task InitAsync();

        protected async Task InitAsync(IEnumerable<string> values)
        {
            if (await this.ExistsBloomFilterAsync())
                return;

            var (Success, LockValue) = await _distributedLocker.Value.LockAsync(this.Name);
            if (!Success)
            {
                await Task.Delay(500);
                await InitAsync(values);
            }

            try
            {
                if (values.IsNotNullOrEmpty())
                {
                    await _redisProvider.Value.BloomReserveAsync(this.Name, this.ErrorRate, this.Capacity);
                    await _redisProvider.Value.BloomAddAsync(this.Name, values);
                }
            }
            finally
            {
                await _distributedLocker.Value.SafedUnLockAsync(this.Name, LockValue);
            }
        }

        protected virtual async Task<bool> ExistsBloomFilterAsync()
            => await _redisProvider.Value.KeyExistsAsync(this.Name);
    }
}
