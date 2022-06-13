namespace Adnc.Shared.Application.BloomFilter;

public sealed class NullBloomFilter : AbstractBloomFilter
{
    public NullBloomFilter(Lazy<IRedisProvider> redisProvider, Lazy<IDistributedLocker> distributedLocker) : base(redisProvider, distributedLocker)
    {
    }

    public override string Name => "null";

    public override double ErrorRate => default;

    public override int Capacity => default;

    public override async Task<bool> AddAsync(string value) => await Task.FromResult(true);

    public override async Task<bool[]> AddAsync(IEnumerable<string> values) => await Task.FromResult(new bool[values.Count()]);

    public override async Task<bool> ExistsAsync(string value) => await Task.FromResult(true);

    public override async Task<bool[]> ExistsAsync(IEnumerable<string> values) => await Task.FromResult(new bool[values.Count()]);

    public override Task InitAsync() => Task.CompletedTask;
}