using Adnc.Infra.EventBus.RabbitMq;

namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册健康监测组件
    /// </summary>
    protected IHealthChecksBuilder AddHealthChecks(
        bool checkingMysql = true,
        bool checkingMongodb = true,
        bool checkingRedis = true,
        bool checkingRabitmq = true)
    {
        var checksBuilder = Services.AddHealthChecks();
        //.AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 200, tags: new[] { "memory" })
        //.AddProcessHealthCheck("ProcessName", p => p.Length > 0) // check if process is running
        //.AddUrlGroup(new Uri("https://localhost:5001/weatherforecast"), "index endpoint")
        //await HttpContextUtility.GetCurrentHttpContext().GetTokenAsync("access_token");
        if (checkingMysql)
        {
            var mysqlConfig = Configuration.GetSection(MysqlConfig.Name).Get<MysqlConfig>();
            if (mysqlConfig is null)
                throw new NullReferenceException("mysqlconfig is null");
            checksBuilder.AddMySql(mysqlConfig.ConnectionString);
        }

        if (checkingMongodb)
        {
            var mongoConfig = Configuration.GetSection(MongoConfig.Name).Get<MongoConfig>();
            if (mongoConfig is null)
                throw new NullReferenceException("mongoConfig is null");
            checksBuilder.AddMongoDb(mongoConfig.ConnectionString);
        }

        if (checkingRedis)
        {
            var redisConfig = Configuration.GetSection(RedisConfig.Name).Get<RedisConfig>();
            if (redisConfig is null)
                throw new NullReferenceException("redisConfig is null");
            checksBuilder.AddRedis(redisConfig.Dbconfig.ConnectionString);
        }

        if (checkingRabitmq)
        {
            var rabitmqConfig = Configuration.GetSection(RabbitMqConfig.Name).Get<RabbitMqConfig>();
            if (rabitmqConfig is null)
                throw new NullReferenceException("rabitmqConfig is null");
            checksBuilder.AddRabbitMQ(provider =>
            {
                return provider.GetRequiredService<IRabbitMqConnection>().Connection;
            });
        }

        return checksBuilder;
    }
}
