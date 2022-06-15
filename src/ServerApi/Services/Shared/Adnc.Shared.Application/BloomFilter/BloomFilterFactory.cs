﻿namespace Adnc.Shared.Application.BloomFilter;

public sealed class BloomFilterFactory
{
    private readonly IEnumerable<IBloomFilter> _instances;
    private readonly RedisConfig _redisConfig;

    public BloomFilterFactory(IEnumerable<IBloomFilter> instances, IConfiguration configuration)
    {
        _instances = instances;
        _redisConfig = configuration.GetRedisSection().Get<RedisConfig>();
    }

    public IBloomFilter Create(string name)
    {
        ArgumentCheck.NotNullOrWhiteSpace(name, nameof(name));

        IBloomFilter bloomFilter;
        if (_redisConfig.EnableBloomFilter)
            bloomFilter = _instances.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
        else
            bloomFilter = _instances.FirstOrDefault(x => x.Name.EqualsIgnoreCase("null"));

        return bloomFilter;
    }
}