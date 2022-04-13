namespace Adnc.Shared.Application.BloomFilter;

public class BloomFilterHostedService : BackgroundService
{
    private readonly ILogger<BloomFilterHostedService> _logger;
    private readonly IEnumerable<IBloomFilter> _bloomFilters;

    public BloomFilterHostedService(ILogger<BloomFilterHostedService> logger
       , IEnumerable<IBloomFilter> bloomFilters)
    {
        _logger = logger;
        _bloomFilters = bloomFilters;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        #region Init BloomFilter

        if (_bloomFilters.IsNotNullOrEmpty())
        {
            foreach (var filter in _bloomFilters)
            {
                await filter.InitAsync();
            }
        }

        #endregion Init BloomFilter
    }
}