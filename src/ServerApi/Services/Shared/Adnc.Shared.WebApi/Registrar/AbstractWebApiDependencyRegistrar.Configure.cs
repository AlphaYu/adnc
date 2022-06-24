using Adnc.Infra.Caching.Configurations;

namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册配置类到IOC容器
    /// </summary>
    protected virtual void Configure()
    {
        Services
            .Configure<JwtConfig>(Configuration.GetSection(JwtConfig.Name))
            .Configure<RedisConfig>(Configuration.GetSection(RedisConfig.Name))
            .Configure<MongoConfig>(Configuration.GetSection(MongoConfig.Name))
            .Configure<MysqlConfig>(Configuration.GetSection(MysqlConfig.Name))
            .Configure<RabbitMqConfig>(Configuration.GetSection(RabbitMqConfig.Name))
            .Configure<ConsulConfig>(Configuration.GetSection(ConsulConfig.Name))
            .Configure<ThreadPoolSettings>(Configuration.GetSection(ThreadPoolSettings.Name))
            .Configure<KestrelConfig>(Configuration.GetSection(KestrelConfig.Name))
            ;
    }
}
