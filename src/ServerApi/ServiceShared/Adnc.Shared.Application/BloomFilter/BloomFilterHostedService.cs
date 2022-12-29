using Microsoft.Extensions.Options;

namespace Adnc.Shared.Application.BloomFilter;

public class BloomFilterHostedService : BackgroundService
{
    private readonly ILogger<BloomFilterHostedService> _logger;
    private readonly IEnumerable<IBloomFilter> _bloomFilters;
    private readonly IOptions<RedisOptions> _redisOptions;

    public BloomFilterHostedService(
        ILogger<BloomFilterHostedService> logger
       , IEnumerable<IBloomFilter> bloomFilters
       , IOptions<RedisOptions> redisOptions)
    {
        _logger = logger;
        _bloomFilters = bloomFilters;
        _redisOptions = redisOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_redisOptions.Value.EnableBloomFilter && _bloomFilters.IsNotNullOrEmpty())
        {
            foreach (var filter in _bloomFilters)
            {
                await filter.InitAsync();
            }
        }
    }
}