namespace Adnc.Infra.Unittest.Redis.Fixtures;

public class RedisContextFixture
{
    private IServiceProvider? container;
    public IServiceProvider Container
    {
        get
        {
            var redisSection = Configuration.GetSection("Redis");
            var cachingSection = Configuration.GetSection("Caching");
            return container ??= new ServiceCollection()
                                        .AddAdncInfraRedis(redisSection)
                                        .AddAdncInfraRedisCaching(null, redisSection, cachingSection)
                                        .AddAdncInfraYitterIdGenerater(redisSection, "unittest")
                                        .BuildServiceProvider();
        }
    }

    public IConfiguration? configuration;
    public IConfiguration Configuration
    {
        get
        {
            return configuration ??= new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", optional: true)
                                        .Build();
        }
    }
}
