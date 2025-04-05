using DotNetCore.CAP;
using RabbitMQ.Client;

namespace Adnc.Infra.EventBus.RabbitMq;

public interface IConnectionManager
{
    IConnection Connection { get; }
}

public sealed class ConnectionManager : IConnectionManager
{
    private static volatile ConnectionManager? _uniqueInstance;
    private static readonly object _lockObject = new();

    private ConnectionManager()
    {
    }

    public IConnection Connection { get; private set; } = default!;

    public static ConnectionManager GetInstance(IOptions<RabbitMQOptions> options, string clientProvidedName, ILogger<IConnectionManager> logger)
    {
        return GetInstance(options.Value, clientProvidedName, logger);
    }

    public static ConnectionManager GetInstance(RabbitMQOptions options, string clientProvidedName, ILogger<IConnectionManager> logger)
    {
        if (_uniqueInstance is null)
        {
            lock (_lockObject)
            {
                _uniqueInstance ??= new ConnectionManager { Connection = CreateConnection(options, clientProvidedName, logger) };
            }
        }

        return _uniqueInstance;
    }

    private static IConnection CreateConnection(RabbitMQOptions options, string clientProvidedName, ILogger<IConnectionManager> logger)
    {
        var factory = new ConnectionFactory()
        {
            Port = options.Port,
            VirtualHost = options.VirtualHost,
            UserName = options.UserName,
            Password = options.Password,
            ClientProvidedName = clientProvidedName
        };

        if (options.HostName.Contains(','))
        {
            logger.LogInformation("create a connection using a list of endpoints");
            //Rabbitmq集群必需加这两个参数
            factory.AutomaticRecoveryEnabled = true;
            //factory.TopologyRecoveryEnabled=true
            return factory.CreateConnectionAsync(AmqpTcpEndpoint.ParseMultiple(options.HostName)).GetAwaiter().GetResult();
        }
        else
        {
            logger.LogInformation(" create a connection to one of the endpoints");
            factory.HostName = options.HostName;
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }
    }
}
