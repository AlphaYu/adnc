namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册健康监测组件
    /// </summary>
    protected virtual void AddHealthChecks()
    {
        var mysqlConfig = Configuration.GetSection(MysqlConfig.Name).Get<MysqlConfig>();
        var mongoConfig = Configuration.GetSection(MongoConfig.Name).Get<MongoConfig>();
        var redisConfig = Configuration.GetSection(RedisConfig.Name).Get<RedisConfig>();
        Services
            .AddHealthChecks()
            //.AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 200, tags: new[] { "memory" })
            //.AddProcessHealthCheck("ProcessName", p => p.Length > 0) // check if process is running
            .AddMySql(mysqlConfig.ConnectionString)
            .AddMongoDb(mongoConfig.ConnectionString)
            .AddRabbitMQ(x =>
            {
                return
                Infra.EventBus.RabbitMq.RabbitMqConnection.GetInstance(x.GetService<IOptionsMonitor<RabbitMqConfig>>()
                    , x.GetService<ILogger<dynamic>>()
                ).Connection;
            })
            //.AddUrlGroup(new Uri("https://localhost:5001/weatherforecast"), "index endpoint")
            //await HttpContextUtility.GetCurrentHttpContext().GetTokenAsync("access_token");
            .AddRedis(redisConfig.dbconfig.ConnectionString);
    }
}
