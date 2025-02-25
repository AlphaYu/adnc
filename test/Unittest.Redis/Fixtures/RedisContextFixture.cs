namespace Adnc.Infra.Unittest.Redis.Fixtures;

public class RedisContextFixture
{
    public IServiceProvider Container { get; private set; }
    public IConfiguration Configuration { get; private set; }

    public RedisContextFixture()
    {
        Configuration = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: true)
                                            .Build();

        var redisSection = Configuration.GetSection("Redis");
        var cachingSection = Configuration.GetSection("Caching");

        var services = new ServiceCollection();
        services
            .AddAdncInfraRedis(redisSection)
            .AddAdncInfraRedisCaching(redisSection, cachingSection)
            .AddAdncInfraYitterIdGenerater(redisSection, "unittest");

        Container = services.BuildServiceProvider();
    }
}
