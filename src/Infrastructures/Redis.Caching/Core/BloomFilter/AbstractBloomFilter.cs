namespace Adnc.Infra.Redis.Caching.Core.BloomFilter;

public interface IBloomFilter
{
    /// <summary>
    /// Filter name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Error rate
    /// </summary>
    double ErrorRate { get; }

    /// <summary>
    /// Capacity
    /// </summary>
    int Capacity { get; }

    /// <summary>
    /// Initializes the Bloom filter.
    /// </summary>
    /// <returns></returns>
    Task InitAsync();

    Task<bool> AddAsync(string value);

    Task<bool[]> AddAsync(IEnumerable<string> values);

    Task<bool> ExistsAsync(string value);

    Task<bool[]> ExistsAsync(IEnumerable<string> values);
}

public abstract class AbstractBloomFilter(Lazy<IRedisProvider> redisProvider, Lazy<IDistributedLocker> distributedLocker) : IBloomFilter
{
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

        return await redisProvider.Value.BfAddAsync(Name, value);
    }

    public virtual async Task<bool[]> AddAsync(IEnumerable<string> values)
    {
        var exists = await ExistsBloomFilterAsync();
        if (!exists)
        {
            throw new ArgumentNullException(Name, $"call {nameof(InitAsync)} methos before");
        }

        return await redisProvider.Value.BfAddAsync(Name, values);
    }

    public virtual async Task<bool> ExistsAsync(string value) => await redisProvider.Value.BfExistsAsync(Name, value);

    public virtual async Task<bool[]> ExistsAsync(IEnumerable<string> values) => await redisProvider.Value.BfExistsAsync(Name, values);

    public abstract Task InitAsync();

    protected async Task InitAsync(IEnumerable<string> values)
    {
        if (await ExistsBloomFilterAsync())
        {
            return;
        }

        var (Success, LockValue) = await distributedLocker.Value.LockAsync(Name);
        if (!Success)
        {
            await Task.Delay(500);
            await InitAsync(values);
        }

        try
        {
            if (values.IsNotNullOrEmpty())
            {
                await redisProvider.Value.BfReserveAsync(Name, ErrorRate, Capacity);
                await redisProvider.Value.BfAddAsync(Name, values);
            }
        }
        finally
        {
            await distributedLocker.Value.SafedUnLockAsync(Name, LockValue);
        }
    }

    protected virtual async Task<bool> ExistsBloomFilterAsync() => await redisProvider.Value.KeyExistsAsync(Name);
}
