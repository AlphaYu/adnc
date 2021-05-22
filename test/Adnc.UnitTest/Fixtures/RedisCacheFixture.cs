using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Configurations;
using Autofac;
using System;

namespace Adnc.UnitTest.Fixtures
{
    public class RedisCacheFixture : IDisposable
    {
        public IContainer Container { get; private set; }

        public RedisCacheFixture()
        {
            var containerBuilder = new ContainerBuilder();
            var redisOptions = new RedisDBOptions() { Password = "football", ConnectionTimeout = 1000 * 20 };
            redisOptions.Endpoints.Add(new ServerEndPoint() { Host = "193.112.75.77", Port = 13379 });

            var cacheOptions = new CacheOptions()
            {
                EnableLogging = true
               ,
                DBConfig = redisOptions
                ,
                PenetrationSetting = new CacheOptions.PenetrationOptions
                {
                    Disable = true
                    ,
                    BloomFilterSetting = new CacheOptions.BloomFilterSetting
                    {
                        Capacity = 10000000
                        ,
                        Name = "adnc:bloomfilter"
                        ,
                        ErrorRate = 0.001
                    }
                }
            };

            containerBuilder.RegisterModule(new AdncInfraCachingModule(cacheOptions));

            Container = containerBuilder.Build();
        }

        public void Dispose()
        {
            this.Container?.Dispose();
        }
    }
}