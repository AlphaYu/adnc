using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册健康监测组件
    /// </summary>
    [Obsolete($"use {nameof(HealthChecksBuilderExtension)}.AddHealthChecks instead")]
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
            checksBuilder.AddMySql(Configuration);
        }

        //if (checkingMongodb)
        //{
        //    var mongoConnectionString = Configuration.GetValue(NodeConsts.MongoDb_ConnectionString, string.Empty);
        //    if (mongoConnectionString.IsNullOrEmpty())
        //        throw new ArgumentNullException("mongoConfig is null");

        //    var mongoUrl = new MongoUrl(mongoConnectionString);
        //    Services.AddSingleton<IMongoClient>(new MongoClient(mongoUrl));
        //    checksBuilder.AddMongoDb(provider => provider.GetRequiredService<IMongoClient>());
        //}

        if (checkingRedis)
        {
            checksBuilder.AddRedis(Configuration);
        }

        if (checkingRabitmq)
        {
            checksBuilder.AddRabbitMQ(Configuration, ServiceInfo.Id);
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
