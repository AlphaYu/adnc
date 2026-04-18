using Adnc.Infra.Core.Guard;
using Adnc.Shared.Application.Extensions;
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;

namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    /// <summary>
    /// Registers the CAP component for the event bus and eventual consistency (distributed transactions).
    /// </summary>
    /// <param name="subscribers"></param>
    /// <param name="failedThresholdCallback"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual void AddCapEventBus(IEnumerable<Type> subscribers, Action<FailedInfo>? failedThresholdCallback = null)
    {
        ArgumentNullException.ThrowIfNull(subscribers, nameof(subscribers));
        Checker.Argument.ThrowIfNullOrCountLEZero(subscribers, nameof(subscribers));

        var connectionString = Configuration.GetValue<string>(NodeConsts.Mysql_ConnectionString) ?? throw new InvalidDataException("MySql ConnectionString is null");
        var rabbitMQOptions = Configuration.GetRequiredSection(NodeConsts.RabbitMq).Get<RabbitMQOptions>() ?? throw new InvalidDataException(nameof(RabbitMQOptions));
        var clientProvidedName = ServiceInfo.Id;
        var version = ServiceInfo.Version;
        var groupName = $"cap.{ServiceInfo.ShortName}.{this.GetEnvShortName()}";
        Services.AddAdncInfraCap(subscribers, capOptions =>
        {
            SetCapBasicInfo(capOptions, version, groupName, failedThresholdCallback);
            SetCapRabbitMQInfo(capOptions, rabbitMQOptions, clientProvidedName);
            SetCapMySqlInfo(capOptions, connectionString);
        }, null, Lifetime);
    }

    protected void SetCapRabbitMQInfo(CapOptions capOptions, RabbitMQOptions rabbitMQOptions, string clientProvidedName)
    {
        capOptions.UseRabbitMQ(mqOptions =>
        {
            mqOptions.HostName = rabbitMQOptions.HostName;
            mqOptions.VirtualHost = rabbitMQOptions.VirtualHost;
            mqOptions.Port = rabbitMQOptions.Port;
            mqOptions.UserName = rabbitMQOptions.UserName;
            mqOptions.Password = rabbitMQOptions.Password;
            mqOptions.ConnectionFactoryOptions = (facotry) =>
            {
                facotry.ClientProvidedName = clientProvidedName;
            };
        });
    }

    protected void SetCapMySqlInfo(CapOptions capOptions, string connectionString)
    {
        capOptions.UseMySql(config =>
        {
            config.ConnectionString = connectionString;
            config.TableNamePrefix = "cap";
        });
    }

    protected void SetCapBasicInfo(CapOptions capOptions, string version, string groupName, Action<FailedInfo>? failedThresholdCallback = null)
    {
        capOptions.Version = version;
        // Default: cap.queue.{assembly name}, mapped to queue names in RabbitMQ.
        capOptions.DefaultGroupName = groupName;
        // Default: 60 seconds for the retry interval.
        // By default, retries start 4 minutes after message publishing or consumption fails
        // to avoid issues caused by delayed message status updates.
        // Failures are retried immediately 3 times first, then the retry polling interval takes effect.
        capOptions.FailedRetryInterval = 60;
        // Default: 50, the maximum retry count. No more retries are attempted after this value is reached.
        capOptions.FailedRetryCount = 50;
        // Default: NULL, callback invoked when retries reach the FailedRetryCount threshold.
        // You can use this callback to receive failure notifications for manual intervention,
        // such as sending an email or text message.
        capOptions.FailedThresholdCallback = failedThresholdCallback;
        // Default: 24 * 3600 seconds (1 day), expiration time for successful messages.
        // After a message is published or consumed successfully, it will be removed from persistence
        // when the SucceedMessageExpiredAfter threshold is reached.
        capOptions.SucceedMessageExpiredAfter = 24 * 3600;
        // Default: 1, the number of consumer threads processing messages in parallel.
        // When this value is greater than 1, message execution order is not guaranteed.
        capOptions.ConsumerThreadCount = 1;
    }
}
