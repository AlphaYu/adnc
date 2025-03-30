namespace Microsoft.Extensions.DependencyInjection;

public static class HealthChecksBuilderExtension
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, Action<IHealthChecksBuilder>? setupAction)
    {
        setupAction?.Invoke(services.AddHealthChecks());
        return services;
    }

    public static IHealthChecksBuilder AddMySql(this IHealthChecksBuilder checksBuilder, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(IConfiguration));
        var mysqlConnectionString = configuration.GetValue<string>(NodeConsts.Mysql_ConnectionString) ?? throw new InvalidDataException($"{nameof(NodeConsts.Mysql_ConnectionString)} is null");
        return checksBuilder.AddMySql(mysqlConnectionString);
    }

    public static IHealthChecksBuilder AddRedis(this IHealthChecksBuilder checksBuilder, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(IConfiguration));
        var redisConfig = configuration.GetSection(NodeConsts.Redis).Get<RedisOptions>() ?? throw new InvalidDataException($"{nameof(NodeConsts.Redis)} is null"); ;
        return checksBuilder.AddRedis(redisConfig.Dbconfig.ConnectionString);
    }

    public static IHealthChecksBuilder AddRabbitMQ(this IHealthChecksBuilder checksBuilder, IConfiguration configuration, string clientProvidedName = "unkonow")
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(IConfiguration));
        var rabitmqConfig = configuration.GetSection(NodeConsts.RabbitMq).Get<Adnc.Infra.EventBus.Configurations.RabbitMqOptions>() ?? throw new InvalidDataException("RabbitMqOptions is null");
        return checksBuilder.AddRabbitMQ(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<IConnectionManager>>();
            var serviceInfo = provider.GetRequiredService<IServiceInfo>();
            return ConnectionManager.GetInstance(rabitmqConfig, clientProvidedName, logger).Connection;
        });
    }
}
