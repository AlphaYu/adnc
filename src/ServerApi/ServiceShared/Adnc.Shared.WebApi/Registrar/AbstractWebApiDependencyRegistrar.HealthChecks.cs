using Adnc.Infra.EventBus.Configurations;
using Adnc.Infra.EventBus.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProtoBuf.Meta;

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
            var mysqlConnectionString = Configuration.GetValue(NodeConsts.Mysql_ConnectionString, string.Empty);
            if (mysqlConnectionString.IsNullOrEmpty())
                throw new NullReferenceException("mysqlconfig is null");
            checksBuilder.AddMySql(mysqlConnectionString);
        }

        if (checkingMongodb)
        {
            var mongoConnectionString = Configuration.GetValue(NodeConsts.MongoDb_ConnectionString, string.Empty);
            if (mongoConnectionString.IsNullOrEmpty())
                throw new NullReferenceException("mongoConfig is null");

            var mongoUrl = new MongoUrl(mongoConnectionString);
            Services.AddSingleton<IMongoClient>(new MongoClient(mongoUrl));
            checksBuilder.AddMongoDb(provider => provider.GetRequiredService<IMongoClient>());
        }

        if (checkingRedis)
        {
            var redisConfig = Configuration.GetSection(NodeConsts.Redis).Get<RedisOptions>();
            if (redisConfig is null)
                throw new NullReferenceException("redisConfig is null");
            checksBuilder.AddRedis(redisConfig.Dbconfig.ConnectionString);
        }

        if (checkingRabitmq)
        {
            var rabitmqConfig = Configuration.GetSection(NodeConsts.RabbitMq).Get<RabbitMqOptions>();
            if (rabitmqConfig is null)
                throw new NullReferenceException("rabitmqConfig is null");

            //var myServer = $"{rabitmqConfig.HostName}:{rabitmqConfig.Port}";
            //var myVirtualHost = rabitmqConfig.VirtualHost;
            //var userName = rabitmqConfig.UserName;
            //var password = rabitmqConfig.Password;
            //ClientProvidedName = serviceInfo.Id,
            //AutomaticRecoveryEnabled = true
            //var connectionstring = $"amqp://host={myServer};virtualHost={myVirtualHost};username={userName};password={password}";
            checksBuilder.AddRabbitMQ(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<dynamic>>();
                var serviceInfo = provider.GetRequiredService<IServiceInfo>();
                var clientProvidedName = serviceInfo.Id ?? "unkonow";
                return RabbitMqConnection.GetInstance(rabitmqConfig, clientProvidedName, logger).Connection;
            });
        }

        return checksBuilder;
    }

    /// <summary>
    /// 注册健康监测组件
    /// </summary>
    protected IHealthChecksBuilder AddHealthChecks()
    {
        var checksBuilder = Services.AddHealthChecks();
        checksBuilder.AddCheck("instance", () => HealthCheckResult.Healthy("instance is ok"));
        return checksBuilder;
    }
}
