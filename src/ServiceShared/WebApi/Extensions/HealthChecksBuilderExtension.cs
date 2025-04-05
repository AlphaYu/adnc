using DotNetCore.CAP;

namespace Microsoft.Extensions.DependencyInjection;

public static class HealthChecksBuilderExtension
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, Action<IHealthChecksBuilder>? setupAction)
    {
        var checksBuilder = services.AddHealthChecks();
        setupAction?.Invoke(checksBuilder);
        return services;
    }

    public static IHealthChecksBuilder AddMySql(this IHealthChecksBuilder checksBuilder, IConfigurationSection mysqlSection)
    {
        ArgumentNullException.ThrowIfNull(mysqlSection, nameof(mysqlSection));
        var mysqlConnectionString = mysqlSection.GetValue<string>(NodeConsts.ConnectionString) ?? throw new InvalidDataException($"{nameof(NodeConsts.ConnectionString)} is null");
        return checksBuilder.AddMySql(mysqlConnectionString);
    }

    public static IHealthChecksBuilder AddRedis(this IHealthChecksBuilder checksBuilder, IConfigurationSection redisSection)
    {
        ArgumentNullException.ThrowIfNull(redisSection, nameof(redisSection));
        var redisConfig = redisSection.Get<RedisOptions>() ?? throw new InvalidDataException($"{nameof(NodeConsts.Redis)} is null");
        var connectionString = redisConfig.Dbconfig.ConnectionString;
        return checksBuilder.AddRedis(connectionString);
    }

    public static IHealthChecksBuilder AddRabbitMQ(this IHealthChecksBuilder checksBuilder, IConfigurationSection rabbitMQSection, string clientProvidedName = "unkonow")
    {
        ArgumentNullException.ThrowIfNull(rabbitMQSection, nameof(rabbitMQSection));
        var rabbitMQOptions = rabbitMQSection.Get<RabbitMQOptions>() ?? throw new InvalidDataException($"{nameof(NodeConsts.RabbitMq)} is null");
        return checksBuilder.AddRabbitMQ(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<IConnectionManager>>();
            var serviceInfo = provider.GetRequiredService<IServiceInfo>();
            return ConnectionManager.GetInstance(rabbitMQOptions, clientProvidedName, logger).Connection;
        });
    }
}
