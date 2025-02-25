namespace Microsoft.Extensions.DependencyInjection;

public static class HealthChecksBuilderExtension
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, Action<IHealthChecksBuilder> setupAction)
    {
        Checker.Argument.NotNull(setupAction, nameof(Action<IHealthChecksBuilder>));

        if (setupAction != null)
            setupAction.Invoke(services.AddHealthChecks());

        return services;
    }

    public static IHealthChecksBuilder AddMySql(this IHealthChecksBuilder checksBuilder, IConfiguration configuration)
    {
        Checker.Argument.NotNull(configuration, nameof(IConfiguration));
        var mysqlConnectionString = configuration.GetValue(NodeConsts.Mysql_ConnectionString, string.Empty);
        Checker.ThrowIfNullOrWhiteSpace(mysqlConnectionString);
        return checksBuilder.AddMySql(mysqlConnectionString);
    }

    public static IHealthChecksBuilder AddRedis(this IHealthChecksBuilder checksBuilder, IConfiguration configuration)
    {
        Checker.Argument.NotNull(configuration, nameof(IConfiguration));
        var redisConfig = configuration.GetSection(NodeConsts.Redis).Get<RedisOptions>();
        return checksBuilder.AddRedis(redisConfig.Dbconfig.ConnectionString);
    }

    public static IHealthChecksBuilder AddRabbitMQ(this IHealthChecksBuilder checksBuilder, IConfiguration configuration, string clientProvidedName = "unkonow")
    {
        Checker.Argument.NotNull(configuration, nameof(IConfiguration));
        var rabitmqConfig = configuration.GetSection(NodeConsts.RabbitMq).Get<Adnc.Infra.EventBus.Configurations.RabbitMqOptions>();
        return checksBuilder.AddRabbitMQ(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<dynamic>>();
            var serviceInfo = provider.GetRequiredService<IServiceInfo>();
            return RabbitMqConnection.GetInstance(rabitmqConfig, clientProvidedName, logger).Connection;
        });
    }
}
