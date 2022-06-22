using Microsoft.Extensions.Options;

namespace Adnc.Shared.Application.BloomFilter;

public class BloomFilterHostedService : BackgroundService
{
    private readonly ILogger<BloomFilterHostedService> _logger;
    private readonly IEnumerable<IBloomFilter> _bloomFilters;
    private readonly RedisConfig _redisConfig;

    public BloomFilterHostedService(ILogger<BloomFilterHostedService> logger
       , IEnumerable<IBloomFilter> bloomFilters
       , IOptionsMonitor<RedisConfig> redisOptions)
    {
        _logger = logger;
        _bloomFilters = bloomFilters;
        _redisConfig = redisOptions.CurrentValue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_redisConfig.EnableBloomFilter && _bloomFilters.IsNotNullOrEmpty())
        {
            foreach (var filter in _bloomFilters)
            {
                await filter.InitAsync();
            }
        }
    }
}