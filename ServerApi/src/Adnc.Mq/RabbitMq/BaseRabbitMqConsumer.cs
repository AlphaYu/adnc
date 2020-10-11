using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Adnc.Infr.Mq.RabbitMq
{
    public abstract class BaseRabbitMqConsumer : IHostedService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ExchangeType _exchangeType;
        private readonly string _exchangeName;
        private readonly string _queueName;
        private readonly string[] _routingKeys;
        private readonly string _deadExchangeName;
        private Dictionary<string, object> _arguments;
        private readonly string _deadQueueName;
        private readonly ILogger<dynamic> _logger;

        public BaseRabbitMqConsumer(IOptions<RabbitMqConfig> options
            , IHostApplicationLifetime appLifetime
            , ILogger<dynamic> logger
            , ExchangeType exchangeType
            , string exchangeName
            , string[] routingKeys
            , string queueName
            , string deadExchangeName = null
            , Dictionary<string, object> arguments = null)
        {
            _appLifetime = appLifetime;
            _exchangeType = exchangeType;
            _exchangeName = exchangeName;
            _queueName = queueName;
            _routingKeys = routingKeys;
            _deadExchangeName = deadExchangeName;
            _deadQueueName = $"dead-letter-{queueName}";
            _arguments = arguments;
            _connection = RabbitMqConnection.GetInstance(options, _logger).Connection;
            _channel = _connection.CreateModel();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_appLifetime.ApplicationStarted.Register(OnStarted);
            //_appLifetime.ApplicationStopping.Register(OnStoping);
            //_appLifetime.ApplicationStopped.Register(OnStopped);
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
            //注册死信交换机
            this.RegiterDeadExchange();

            //声明交换机
            _channel.ExchangeDeclare(_exchangeName, type: _exchangeType.ToString().ToLower());

            //声明一个队列
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: _arguments);

            //将队列与交换机进行绑定
            if (_routingKeys == null || _routingKeys.Length == 0)
            {
                _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: string.Empty);
            }
            else
            {
                foreach (var routingKey in _routingKeys)
                {
                    _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: routingKey);
                }
            }
            //每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var result = await Process(ea.Exchange, ea.RoutingKey, message);
                if (result)
                {
                    var multiple = false;
                    _channel.BasicAck(ea.DeliveryTag, multiple);
                }
                else
                {
                    //requeue = true,重新放回队列
                    //requeue = false,如果配置死信队列，会转义到死信队列,没有则丢弃。
                    _channel.BasicReject(ea.DeliveryTag, requeue: false);
                }
            };

            //autoAck=false 关闭自动确认,开启手动确认。
            _channel.BasicConsume(queue: _queueName, consumer: consumer, autoAck: false);
        }

        /// <summary>
        /// 注销/关闭连接
        /// </summary>
        protected virtual void DeRegister()
        {
            if (_connection != null)
                _connection.Dispose();
        }


        /// <summary>
        /// 处理消息的方法
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract Task<bool> Process(string exchange, string routingKey, string message);

        //private void OnStarted()
        //{
        //    _logger.LogInformation("OnStarted has been called.");
        //}

        //private void OnStoping()
        //{
        //    _logger.LogInformation("OnStoping has been called.");
        //}

        //private void OnStopped()
        //{
        //    _logger.LogInformation("OnStopped has been called.");
        //}

        protected virtual void RegiterDeadExchange()
        {
            if (!string.IsNullOrWhiteSpace(_deadExchangeName))
            {
                _channel.ExchangeDeclare(_deadExchangeName, ExchangeType.Direct.ToString().ToLower());
                _channel.QueueDeclare(queue: _deadQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                foreach (var routingKey in _routingKeys)
                {
                    _channel.QueueBind(queue: _deadQueueName, exchange: _deadExchangeName, routingKey: routingKey);
                }
            }
        }
    }
}
