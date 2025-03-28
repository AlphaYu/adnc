﻿namespace Adnc.Shared.Application.BloomFilter;

public interface IBloomFilter
{
    /// <summary>
    /// 过滤器名字
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 容错率
    /// </summary>
    double ErrorRate { get; }

    /// <summary>
    /// 容积
    /// </summary>
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
        var exists = await ExistsBloomFilterAsync();
        if (!exists)
        {
            throw new ArgumentNullException(Name, $"call {nameof(InitAsync)} methos before");
        }

        return await _redisProvider.Value.BfAddAsync(Name, value);
    }

    public virtual async Task<bool[]> AddAsync(IEnumerable<string> values)
    {
        var exists = await ExistsBloomFilterAsync();
        if (!exists)
        {
            throw new ArgumentNullException(Name, $"call {nameof(InitAsync)} methos before");
        }

        return await _redisProvider.Value.BfAddAsync(Name, values);
    }

    public virtual async Task<bool> ExistsAsync(string value) => await _redisProvider.Value.BfExistsAsync(Name, value);

    public virtual async Task<bool[]> ExistsAsync(IEnumerable<string> values) => await _redisProvider.Value.BfExistsAsync(Name, values);

    public abstract Task InitAsync();

    protected async Task InitAsync(IEnumerable<string> values)
    {
        if (await ExistsBloomFilterAsync())
        {
            return;
        }

        var (Success, LockValue) = await _distributedLocker.Value.LockAsync(Name);
        if (!Success)
        {
            await Task.Delay(500);
            await InitAsync(values);
        }

        try
        {
            if (values.IsNotNullOrEmpty())
            {
                await _redisProvider.Value.BfReserveAsync(Name, ErrorRate, Capacity);
                await _redisProvider.Value.BfAddAsync(Name, values);
            }
        }
        finally
        {
            await _distributedLocker.Value.SafedUnLockAsync(Name, LockValue);
        }
    }

    protected virtual async Task<bool> ExistsBloomFilterAsync() => await _redisProvider.Value.KeyExistsAsync(Name);
}