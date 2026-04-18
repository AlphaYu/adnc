using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Adnc.Infra.EventBus.RabbitMq;

public abstract class BaseRabbitMqConsumer(IConnectionManager connectionManager, ILogger<dynamic> logger) : IHostedService
{
    private IChannel? _normalChannel;
    private IChannel? _deadChannel;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await RegisterAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await DeRegisterAsync();
    }

    /// <summary>
    /// Registers the consumer.
    /// </summary>
    protected virtual async Task RegisterAsync()
    {
        // Get exchange configuration
        var exchange = GetExchageConfig();

        // Get routing keys
        var routingKeys = GetRoutingKeys();

        // Get queue configuration
        var queue = GetQueueConfig();

        // Declare the dead-letter exchange and queue
        await RegiterDeadExchange(exchange.DeadExchangeName, queue.DeadQueueName, routingKeys, queue.Durable);

        // Declare the exchange
        _normalChannel = await connectionManager.Connection.CreateChannelAsync();
        await _normalChannel.ExchangeDeclareAsync(exchange.Name, type: exchange.Type.ToString().ToLower());

        // Declare the queue
        await _normalChannel.QueueDeclareAsync(queue: queue.Name
            , durable: queue.Durable
            , exclusive: queue.Exclusive
            , autoDelete: queue.AutoDelete
            , arguments: queue.Arguments
        );

        // Bind the queue to the exchange
        if (routingKeys == null || routingKeys.Length == 0)
        {
            await _normalChannel.QueueBindAsync(queue: queue.Name, exchange: exchange.Name, routingKey: string.Empty);
        }
        else
        {
            foreach (var key in routingKeys)
            {
                await _normalChannel.QueueBindAsync(queue: queue.Name, exchange: exchange.Name, routingKey: key);
            }
        }

        var consumer = new AsyncEventingBasicConsumer(_normalChannel);

        // Disable auto-ack; when manual ack is enabled, configure QoS and consume
        if (!queue.AutoAck)
        {
            await _normalChannel.BasicQosAsync(prefetchSize: 0, prefetchCount: queue.PrefetchCount, global: queue.Global);
            await _normalChannel.BasicConsumeAsync(queue: queue.Name, consumer: consumer, autoAck: queue.AutoAck);
        }

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var result = await Process(ea.Exchange, ea.RoutingKey, message);

            logger.LogDebug("result:{result},message:{message}", result, message);

            // When manual ack is enabled, choose the ack response based on the processing result
            if (!queue.AutoAck)
            {
                if (result)
                {
                    await _normalChannel.BasicAckAsync(ea.DeliveryTag, multiple: queue.AckMultiple);
                }
                else
                {
                    await _normalChannel.BasicRejectAsync(ea.DeliveryTag, requeue: queue.RejectRequeue);
                }
            }
        };
    }

    /// <summary>
    /// Deregisters / closes connections.
    /// </summary>
    protected virtual async Task DeRegisterAsync()
    {
        if (_normalChannel is not null)
        {
            await _normalChannel.DisposeAsync();
        }
        if (_deadChannel is not null)
        {
            await _deadChannel.DisposeAsync();
        }
        if (connectionManager.Connection is not null)
        {
            await connectionManager.Connection.DisposeAsync();
        }
    }

    /// <summary>
    /// Processes a received message.
    /// </summary>
    /// <param name="exchange"></param>
    /// <param name="routingKey"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    protected abstract Task<bool> Process(string exchange, string routingKey, string message);

    /// <summary>
    /// Gets the exchange configuration.
    /// </summary>
    /// <returns></returns>
    protected abstract ExchageConfig GetExchageConfig();

    /// <summary>
    /// Gets the routing keys.
    /// </summary>
    /// <returns></returns>
    protected abstract string[] GetRoutingKeys();

    /// <summary>
    /// Gets the queue configuration.
    /// </summary>
    /// <returns></returns>
    protected abstract QueueConfig GetQueueConfig();

    /// <summary>
    /// Gets the common queue configuration.
    /// </summary>
    /// <returns></returns>
    protected QueueConfig GetCommonQueueConfig()
    {
        return new QueueConfig()
        {
            Name = string.Empty
            ,
            AutoDelete = false
            ,
            Durable = false
            ,
            Exclusive = false
            ,
            Global = true
            ,
            AutoAck = false
            ,
            AckMultiple = false
            ,
            PrefetchCount = 1
            ,
            RejectRequeue = false
            ,
            Arguments = null
        };
    }

    /// <summary>
    /// Declares the dead-letter exchange and queue.
    /// </summary>
    protected virtual async Task RegiterDeadExchange(string deadExchangeName, string deadQueueName, string[] routingKeys, bool durable)
    {
        if (!string.IsNullOrWhiteSpace(deadExchangeName))
        {
            _deadChannel = await connectionManager.Connection.CreateChannelAsync();
            await _deadChannel.ExchangeDeclareAsync(deadExchangeName, ExchangeType.Direct.ToString().ToLower());
            await _deadChannel.QueueDeclareAsync(queue: deadQueueName, durable: durable, exclusive: false, autoDelete: false, arguments: null);
            foreach (var key in routingKeys)
            {
                await _deadChannel.QueueBindAsync(queue: deadQueueName, exchange: deadExchangeName, routingKey: key);
            }
        }
    }
}
