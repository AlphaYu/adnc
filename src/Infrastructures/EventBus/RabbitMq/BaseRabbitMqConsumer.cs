using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Adnc.Infra.EventBus.RabbitMq;

public abstract class BaseRabbitMqConsumer(IConnectionManager connectionManager, ILogger<dynamic> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await RegisterAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await DeRegisterAsync();
    }

    /// <summary>
    /// 注册消费者
    /// </summary>
    protected virtual async Task RegisterAsync()
    {
        //获取交换机配置
        var exchange = GetExchageConfig();

        //获取routingKeys
        var routingKeys = GetRoutingKeys();

        //获取队列配置
        var queue = GetQueueConfig();

        //声明死信交换与队列
        await RegiterDeadExchange(exchange.DeadExchangeName, queue.DeadQueueName, routingKeys, queue.Durable);

        //声明交换机
        using var channel = await connectionManager.Connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange.Name, type: exchange.Type.ToString().ToLower());

        //声明队列
        await channel.QueueDeclareAsync(queue: queue.Name
            , durable: queue.Durable
            , exclusive: queue.Exclusive
            , autoDelete: queue.AutoDelete
            , arguments: queue.Arguments
        );

        //将队列与交换机进行绑定
        if (routingKeys == null || routingKeys.Length == 0)
        {
            await channel.QueueBindAsync(queue: queue.Name, exchange: exchange.Name, routingKey: string.Empty);
        }
        else
        {
            foreach (var key in routingKeys)
            {
                await channel.QueueBindAsync(queue: queue.Name, exchange: exchange.Name, routingKey: key);
            }
        }

        var consumer = new AsyncEventingBasicConsumer(channel);

        //关闭自动确认,开启手动确认后需要配置这些
        if (!queue.AutoAck)
        {
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: queue.PrefetchCount, global: queue.Global);
            await channel.BasicConsumeAsync(queue: queue.Name, consumer: consumer, autoAck: queue.AutoAck);
        }

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var result = await Process(ea.Exchange, ea.RoutingKey, message);

            logger.LogDebug("result:{result},message:{message}", result, message);

            //关闭自动确认,开启手动确认后需要依据处理结果选择返回确认信息。
            if (!queue.AutoAck)
            {
                if (result)
                {
                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: queue.AckMultiple);
                }
                else
                {
                    await channel.BasicRejectAsync(ea.DeliveryTag, requeue: queue.RejectRequeue);
                }
            }
        };
    }

    /// <summary>
    /// 注销/关闭连接
    /// </summary>
    protected virtual async Task DeRegisterAsync()
    {
        if (connectionManager.Connection != null)
        {
            await connectionManager.Connection.DisposeAsync();
        }
    }

    /// <summary>
    /// 处理消息的方法
    /// </summary>
    /// <param name="exchange"></param>
    /// <param name="routingKey"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    protected abstract Task<bool> Process(string exchange, string routingKey, string message);

    /// <summary>
    /// 获取交互机列配置
    /// </summary>
    /// <returns></returns>
    protected abstract ExchageConfig GetExchageConfig();

    /// <summary>
    /// 获取路由keys
    /// </summary>
    /// <returns></returns>
    protected abstract string[] GetRoutingKeys();

    /// <summary>
    /// 获取队列配置
    /// </summary>
    /// <returns></returns>
    protected abstract QueueConfig GetQueueConfig();

    /// <summary>
    /// 获取队列公共配置
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
    /// 声明死信交换与队列
    /// </summary>
    protected virtual async Task RegiterDeadExchange(string deadExchangeName, string deadQueueName, string[] routingKeys, bool durable)
    {
        if (!string.IsNullOrWhiteSpace(deadExchangeName))
        {
            using var channel = await connectionManager.Connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(deadExchangeName, ExchangeType.Direct.ToString().ToLower());
            await channel.QueueDeclareAsync(queue: deadQueueName, durable: durable, exclusive: false, autoDelete: false, arguments: null);
            foreach (var key in routingKeys)
            {
                await channel.QueueBindAsync(queue: deadQueueName, exchange: deadExchangeName, routingKey: key);
            }
        }
    }
}
