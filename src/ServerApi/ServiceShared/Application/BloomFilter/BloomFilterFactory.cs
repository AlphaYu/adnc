using Microsoft.Extensions.Options;

namespace Adnc.Shared.Application.BloomFilter;

public sealed class BloomFilterFactory
{
    private readonly IEnumerable<IBloomFilter> _instances;
    private readonly IOptions<RedisOptions> _redisOptions;

    public BloomFilterFactory(
        IEnumerable<IBloomFilter> instances
        , IOptions<RedisOptions> redisOptions)
    {
        _instances = instances;
        _redisOptions = redisOptions;
    }

    public IBloomFilter Create(string name)
    {
        IBloomFilter? bloomFilter;
        if (_redisOptions.Value.EnableBloomFilter)
            bloomFilter = _instances.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        else
            bloomFilter = _instances.FirstOrDefault(x => x.Name.EqualsIgnoreCase("null"));

        return bloomFilter ?? throw new NullReferenceException(nameof(bloomFilter));
    }
}