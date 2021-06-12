using Adnc.Application.Shared.Caching;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Application.Shared.HostedServices
{
    public class CacheAndBloomFilterHostedService : BackgroundService
    {
        private readonly ILogger<CacheAndBloomFilterHostedService> _logger;
        private readonly ICacheProvider _cache;
        private readonly IEnumerable<IBloomFilter> _bloomFilters;
        private readonly ICacheService _cacheService;

        public CacheAndBloomFilterHostedService(ILogger<CacheAndBloomFilterHostedService> logger
           , ICacheProvider cache
           , IEnumerable<IBloomFilter> bloomFilters
           , ICacheService cacheService)
        {
            _logger = logger;
            _cache = cache;
            _bloomFilters = bloomFilters;
            _cacheService = cacheService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            #region Init BloomFilter

            if (_bloomFilters?.Any() == true)
            {
                foreach (var filter in _bloomFilters)
                    await filter.InitAsync();
            }

            #endregion Init BloomFilter

            #region Init Caches

            await _cacheService.PreheatAsync();

            #endregion Init Caches

            #region Confirm Caching Removed

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!LocalVariables.Instance.Queue.TryDequeue(out LocalVariables.Model model)
                    || model.CacheKeys?.Any() == false
                    || DateTime.Now > model.ExpireDt)
                {
                    await Task.Delay(_cache.CacheOptions.LockMs, stoppingToken);
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
                        await Task.Delay(_cache.CacheOptions.LockMs, stoppingToken);
                    }
                }
            }

            #endregion Confirm Caching Removed
        }
    }
}