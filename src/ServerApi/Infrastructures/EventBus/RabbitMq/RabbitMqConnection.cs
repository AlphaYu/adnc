using Adnc.Infra.EventBus.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Adnc.Infra.EventBus.RabbitMq
{
    public interface IRabbitMqConnection
    {
        IConnection Connection { get; }
    }

    public sealed class RabbitMqConnection : IRabbitMqConnection
    {
        private static volatile RabbitMqConnection? _uniqueInstance;
        private static readonly object _lockObject = new();
        private ILogger<dynamic> _logger = default!;
        public IConnection Connection { get; private set; } = default!;

        private RabbitMqConnection()
        {
        }

        public static RabbitMqConnection GetInstance(IOptions<RabbitMqOptions> options, string clientProvidedName, ILogger<dynamic> logger)
        {
            if (_uniqueInstance is null)
            {
                lock (_lockObject)
                {
                    if (_uniqueInstance is null)
                    {
                        _uniqueInstance = new RabbitMqConnection(options.Value, clientProvidedName, logger);
                    }
                }
            }
            return _uniqueInstance;
        }

        public static RabbitMqConnection GetInstance(RabbitMqOptions options, string clientProvidedName, ILogger<dynamic> logger)
        {
            if (_uniqueInstance is null)
            {
                lock (_lockObject)
                {
                    if (_uniqueInstance is null)
                    {
                        _uniqueInstance = new RabbitMqConnection(options, clientProvidedName, logger);
                    }
                }
            }
            return _uniqueInstance;
        }

        private RabbitMqConnection(RabbitMqOptions options, string clientProvidedName, ILogger<dynamic> logger)
        {
            _logger = logger;

            var factory = new ConnectionFactory()
            {
                ClientProvidedName = clientProvidedName,
                HostName = options.HostName,
                VirtualHost = options.VirtualHost,
                UserName = options.UserName,
                Password = options.Password,
                Port = options.Port,
                //Rabbitmq集群必需加这两个参数
                AutomaticRecoveryEnabled = true,
                //TopologyRecoveryEnabled=true
            };

            Policy.Handle<SocketException>()
                  .Or<BrokerUnreachableException>()
                  .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(1), (ex, time, retryCount, content) =>
                  {
                      if (2 == retryCount)
                          throw ex;
                      _logger.LogError(ex, string.Format("{0}:{1}", retryCount, ex.Message));
                  })
                  .Execute(() =>
                  {
                      Connection = factory.CreateConnectionAsync().Result;
                  });
        }
    }
}