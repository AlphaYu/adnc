using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Core;

namespace Adnc.Application.Shared.IdGeneraterWorkerNode
{
    public class CachingHostedService : BackgroundService
    {
        private readonly ILogger<CachingHostedService> _logger;
        private readonly ICacheProvider _cache;
        private readonly IBloomFilter _bloomFilter;

        public CachingHostedService(ILogger<CachingHostedService> logger
           , ICacheProvider cache
           , IBloomFilter bloomFilter)
        {
            _logger = logger;
            _cache = cache;
            _bloomFilter = bloomFilter;
        }

        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);

            var filter = _cache.CacheOptions.PenetrationSetting.BloomFilter;
            if (!await _cache.ExistsAsync(filter.Name))
            {
                await _bloomFilter.BloomReserveAsync(filter.Name, filter.ErrorRate, filter.Capacity);
            }
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var millisecondsDelay = 300;

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!LocalVariables.Instance.Queue.TryDequeue(out LocalVariables.Model model)
                    || model.CacheKeys?.Any() == false
                    || DateTime.Now > model.ExpireDt)
                {
                    await Task.Delay(millisecondsDelay, stoppingToken);
                    continue;
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        if (DateTime.Now > model.ExpireDt) break;

                        await _cache.RemoveAllAsync(model.CacheKeys);

                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        await Task.Delay(millisecondsDelay, stoppingToken);
                    }
                }
            }
        }
    }
}
