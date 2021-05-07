using System;
using Autofac;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Configurations;

namespace Adnc.UnitTest.Fixtures
{
    public class RedisCacheFixture : IDisposable
    {
        public IContainer Container { get; private set; }

        public RedisCacheFixture()
        {
            var containerBuilder = new ContainerBuilder();
            var redisOptions = new RedisDBOptions() { Password = "football" };
            redisOptions.Endpoints.Add(new ServerEndPoint() { Host = "193.112.75.77", Port = 13379 });

            var cacheOptions = new CacheOptions()
            {
                EnableLogging = true
               ,
                DBConfig = redisOptions
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
