using Microsoft.Extensions.Options;

namespace Adnc.Shared.Application.BloomFilter;

public class BloomFilterHostedService(IEnumerable<IBloomFilter> bloomFilters, IOptions<RedisOptions> redisOptions) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (redisOptions.Value.EnableBloomFilter && bloomFilters.IsNotNullOrEmpty())
        {
            foreach (var filter in bloomFilters)
            {
                await filter.InitAsync();
            }
        }
    }
}
