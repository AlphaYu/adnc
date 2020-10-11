using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Polly;
using RabbitMQ.Client;
using System;

namespace Adnc.Infr.Mq.RabbitMq
{
    public class RabbitMqProducer
    {
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqProducer> _logger;

        public RabbitMqProducer(IOptions<RabbitMqConfig> options, ILogger<RabbitMqProducer> logger)
        {
            _logger = logger;
            _channel = RabbitMqConnection.GetInstance(options, logger).Connection.CreateModel();
        }

        /// <summary>
        /// 简单队列,不通过交换机
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        //public virtual void BasicPublish<TMessage>(string queueName, TMessage message)
        //{
        //    _logger.LogInformation($"PushMessage,queueName:{queueName}");

        //    _channel.QueueDeclare(queue: queueName,
        //                          durable: false,
        //                          exclusive: false,
        //                          autoDelete: false,
        //                          arguments: null);

        //    var content = JsonSerializer.Serialize(message);
        //    var body = Encoding.UTF8.GetBytes(content);

        //    _channel.BasicPublish(exchange: string.Empty,
        //                          routingKey: queueName,
        //                          basicProperties: null,
        //                          body: body);
        //}

        public virtual void BasicPublish<TMessage>(string exchange, string routingKey, TMessage message)
        {
            Policy.Handle<Exception>()
                  .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(1), (ex, time, retryCount, content) =>
                  {
                      _logger.LogError(ex, string.Format("{0}:{1}", retryCount, ex.Message));
                  })
                  .Execute(() =>
                  {
                      var content = message as string;
                      if (content == null)
                          content = JsonSerializer.Serialize(message);

                      var body = Encoding.UTF8.GetBytes(content);

                      _channel.BasicPublish(exchange, routingKey, basicProperties: null, body);
                  });
        }
    }
}
