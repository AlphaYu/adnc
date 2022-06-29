using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Adnc.Infra.EventBus.RabbitMq
{
    public abstract class BaseRabbitMqConsumer : IHostedService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<dynamic> _logger;

        protected BaseRabbitMqConsumer(IRabbitMqConnection RabbitMqConnection, ILogger<dynamic> logger)
        {
            _connection = RabbitMqConnection.Connection;
            _channel = _connection.CreateModel();
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            DeRegister();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 注册消费者
        /// </summary>
        protected virtual void Register()
        {
            //获取交换机配置
            var exchange = GetExchageConfig();

            //获取routingKeys
            var routingKeys = GetRoutingKeys();

            //获取队列配置
            var queue = GetQueueConfig();

            //声明死信交换与队列
            this.RegiterDeadExchange(exchange.DeadExchangeName, queue.DeadQueueName, routingKeys, queue.Durable);

            //声明交换机
            _channel.ExchangeDeclare(exchange.Name, type: exchange.Type.ToString().ToLower());

            //声明队列
            _channel.QueueDeclare(queue: queue.Name
                , durable: queue.Durable
                , exclusive: queue.Exclusive
                , autoDelete: queue.AutoDelete
                , arguments: queue.Arguments
            );

            //将队列与交换机进行绑定
            if (routingKeys == null || routingKeys.Length == 0)
            {
                _channel.QueueBind(queue: queue.Name, exchange: exchange.Name, routingKey: string.Empty);
            }
            else
            {
                foreach (var key in routingKeys)
                {
                    _channel.QueueBind(queue: queue.Name, exchange: exchange.Name, routingKey: key);
                }
            }

            var consumer = new EventingBasicConsumer(_channel);

            //关闭自动确认,开启手动确认后需要配置这些
            if (!queue.AutoAck)
            {
                _channel.BasicQos(prefetchSize: 0, prefetchCount: queue.PrefetchCount, global: queue.Global);
                _channel.BasicConsume(queue: queue.Name, consumer: consumer, autoAck: queue.AutoAck);
            }

            consumer.Received += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var result = await Process(ea.Exchange, ea.RoutingKey, message);

                _logger.LogDebug($"result:{result},message:{message}");

                //关闭自动确认,开启手动确认后需要依据处理结果选择返回确认信息。
                if (!queue.AutoAck)
                    if (result)
                        _channel.BasicAck(ea.DeliveryTag, multiple: queue.AckMultiple);
                    else
                        _channel.BasicReject(ea.DeliveryTag, requeue: queue.RejectRequeue);
            };
        }

        /// <summary>
        /// 注销/关闭连接
        /// </summary>
        protected virtual void DeRegister()
        {
            if (_channel != null)
                _channel.Dispose();
            if (_connection != null)
                _connection.Dispose();
        }

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
        /// 处理消息的方法
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract Task<bool> Process(string exchange, string routingKey, string message);

        /// <summary>
        /// 声明死信交换与队列
        /// </summary>
        protected virtual void RegiterDeadExchange(string deadExchangeName, string deadQueueName, string[] routingKeys, bool durable)
        {
            if (!string.IsNullOrWhiteSpace(deadExchangeName))
            {
                _channel.ExchangeDeclare(deadExchangeName, ExchangeType.Direct.ToString().ToLower());
                _channel.QueueDeclare(queue: deadQueueName, durable: durable, exclusive: false, autoDelete: false, arguments: null);
                foreach (var key in routingKeys)
                {
                    _channel.QueueBind(queue: deadQueueName, exchange: deadExchangeName, routingKey: key);
                }
            }
        }
    }
}