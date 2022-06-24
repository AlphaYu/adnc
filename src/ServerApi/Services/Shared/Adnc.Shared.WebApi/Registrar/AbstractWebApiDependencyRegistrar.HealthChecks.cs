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
        var mysqlConfig = Configuration.GetSection(MysqlConfig.Name).Get<MysqlConfig>();
        var mongoConfig = Configuration.GetSection(MongoConfig.Name).Get<MongoConfig>();
        var redisConfig = Configuration.GetSection(RedisConfig.Name).Get<RedisConfig>();
        var checksBuilder = Services.AddHealthChecks();
        //.AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 200, tags: new[] { "memory" })
        //.AddProcessHealthCheck("ProcessName", p => p.Length > 0) // check if process is running
        //.AddUrlGroup(new Uri("https://localhost:5001/weatherforecast"), "index endpoint")
        //await HttpContextUtility.GetCurrentHttpContext().GetTokenAsync("access_token");
        if (checkingMysql)
            checksBuilder.AddMySql(mysqlConfig.ConnectionString);
        if (checkingMongodb)
            checksBuilder.AddMongoDb(mongoConfig.ConnectionString);
        if (checkingRedis)
            checksBuilder.AddRedis(redisConfig.Dbconfig.ConnectionString);
        if (checkingRabitmq)
            checksBuilder.AddRabbitMQ(x =>
            {
                return
                Infra.EventBus.RabbitMq.RabbitMqConnection.GetInstance(x.GetService<IOptionsMonitor<RabbitMqConfig>>()
                    , x.GetService<ILogger<dynamic>>()
                ).Connection;
            });

        return checksBuilder;
    }
}
