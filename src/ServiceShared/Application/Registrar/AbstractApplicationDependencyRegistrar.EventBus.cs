using Adnc.Infra.Core.Guard;
using Adnc.Shared.Application.Extensions;
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;

namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    /// <summary>
    /// 注册CAP组件(实现事件总线及最终一致性（分布式事务）的一个开源的组件)
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
        //默认值：cap.queue.{程序集名称},在 RabbitMQ 中映射到 Queue Names。
        capOptions.DefaultGroupName = groupName;
        //默认值：60 秒,重试 & 间隔
        //在默认情况下，重试将在发送和消费消息失败的 4分钟后 开始，这是为了避免设置消息状态延迟导致可能出现的问题。
        //发送和消费消息的过程中失败会立即重试 3 次，在 3 次以后将进入重试轮询，此时 FailedRetryInterval 配置才会生效。
        capOptions.FailedRetryInterval = 60;
        //默认值：50,重试的最大次数。当达到此设置值时，将不会再继续重试，通过改变此参数来设置重试的最大次数。
        capOptions.FailedRetryCount = 50;
        //默认值：NULL,重试阈值的失败回调。当重试达到 FailedRetryCount 设置的值的时候，将调用此 Action 回调
        //，你可以通过指定此回调来接收失败达到最大的通知，以做出人工介入。例如发送邮件或者短信。
        capOptions.FailedThresholdCallback = failedThresholdCallback;
        //默认值：24*3600 秒（1天后),成功消息的过期时间（秒）。
        //当消息发送或者消费成功时候，在时间达到 SucceedMessageExpiredAfter 秒时候将会从 Persistent 中删除，你可以通过指定此值来设置过期的时间。
        capOptions.SucceedMessageExpiredAfter = 24 * 3600;
        //默认值：1,消费者线程并行处理消息的线程数，当这个值大于1时，将不能保证消息执行的顺序。
        capOptions.ConsumerThreadCount = 1;
    }
}
