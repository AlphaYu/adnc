namespace Adnc.UnitTest.Fixtures;

public class RedisCacheFixture
{
    public IServiceProvider Container { get; private set; }

    public RedisCacheFixture()
    {
        var services = new ServiceCollection();
        var redisOptions = new RedisDBOptions() { Password = "football", ConnectionTimeout = 1000 * 20 };
        redisOptions.Endpoints.Add(new ServerEndPoint() { Host = "114.132.157.167", Port = 13379 });
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
        services.ConfigureOptions(cacheOptions);
        //services.AddAdncInfraCaching(cacheOptions);

        Container = services.BuildServiceProvider();
    }
}