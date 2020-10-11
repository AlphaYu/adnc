using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Adnc.Application;
using EasyCaching.InMemory;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds and configures the consistence services for the consistency.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configuration"></param>
        public static void AddCustomEasyCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEasyCaching(options =>
            {
                // use memory cache with your own configuration
                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        // scan time, default value is 60s
                        ExpirationScanFrequency = 60,
                        // total count of cache items, default value is 10000
                        SizeLimit = 100,

                        // below two settings are added in v0.8.0
                        // enable deep clone when reading object from cache or not, default value is true.
                        EnableReadDeepClone = true,
                        // enable deep clone when writing object to cache or not, default valuee is false.
                        EnableWriteDeepClone = false,
                    };
                    // the max random second will be added to cache's expiration, default value is 120
                    config.MaxRdSecond = 120;
                    // whether enable logging, default is false
                    config.EnableLogging = false;
                    // mutex key's alive time(ms), default is 5000
                    config.LockMs = 5000;
                    // when mutex key alive, it will sleep some time, default is 300
                    config.SleepMs = 300;
                }, EasyCachingConsts.LocalCaching);

                //Important step for Redis Caching
                options.UseCSRedis(configuration, EasyCachingConsts.RemoteCaching, "Redis");

                // combine local and distributed
                options.UseHybrid(config =>
                {
                    config.TopicName = EasyCachingConsts.TopicName;
                    config.EnableLogging = true;

                    // specify the local cache provider name after v0.5.4
                    config.LocalCacheProviderName = EasyCachingConsts.LocalCaching;
                    // specify the distributed cache provider name after v0.5.4
                    config.DistributedCacheProviderName = EasyCachingConsts.RemoteCaching;
                }, EasyCachingConsts.HybridCaching)
                // use csredis bus
                .WithCSRedisBus(busConf =>
                {
                    busConf.ConnectionStrings = configuration.GetSection("Redis:dbConfig:ConnectionStrings").Get<List<string>>();
                });
            });

            services.ConfigureCastleInterceptor(options =>
            {
                options.CacheProviderName = EasyCachingConsts.HybridCaching;
            });
        }
    }
}
