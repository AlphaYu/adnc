namespace Adnc.Shared.Application.BloomFilter;

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
        var exists = await this.ExistsBloomFilterAsync();
        if (!exists)
            throw new ArgumentNullException(this.Name, $"call {nameof(InitAsync)} methos before");

        return await _redisProvider.Value.BfAddAsync(this.Name, value);
    }

    public virtual async Task<bool[]> AddAsync(IEnumerable<string> values)
    {
        var exists = await this.ExistsBloomFilterAsync();
        if (!exists)
            throw new ArgumentNullException(this.Name, $"call {nameof(InitAsync)} methos before");

        return await _redisProvider.Value.BfAddAsync(this.Name, values);
    }

    public virtual async Task<bool> ExistsAsync(string value)   => await _redisProvider.Value.BfExistsAsync(this.Name, value);

    public virtual async Task<bool[]> ExistsAsync(IEnumerable<string> values) => await _redisProvider.Value.BfExistsAsync(this.Name, values);

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
                await _redisProvider.Value.BfReserveAsync(this.Name, this.ErrorRate, this.Capacity);
                await _redisProvider.Value.BfAddAsync(this.Name, values);
            }
        }
        finally
        {
            await _distributedLocker.Value.SafedUnLockAsync(this.Name, LockValue);
        }
    }

    protected virtual async Task<bool> ExistsBloomFilterAsync() => await _redisProvider.Value.KeyExistsAsync(this.Name);
}