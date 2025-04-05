using RabbitMQ.Client;

namespace Adnc.Infra.EventBus.RabbitMq;

public class RabbitMqProducer(IConnectionManager connectionManager, ILogger<RabbitMqProducer> logger)
{
    /*
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
    */

    public virtual Task BasicPublishAsync<TMessage>(string exchange
        , string routingKey
        , TMessage message
        , BasicProperties? basicProperties = null
        , bool mandatory = false
        , CancellationToken cancellationToken = default
    )
    {
        Policy.Handle<Exception>()
              .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(1), (ex, time, retryCount, content) =>
              {
                  logger.LogError(ex, "Policy.Handle.RetryCount：{retryCount}", retryCount);
              })
              .Execute(async () =>
              {
                  var content = message as string ?? JsonSerializer.Serialize(message);
                  var body = Encoding.UTF8.GetBytes(content);
                  //当mandatory标志位设置为true时，如果exchange根据自身类型和消息routingKey无法找到一个合适的queue存储消息
                  //那么broker会调用basic.return方法将消息返还给生产者;
                  //当mandatory设置为false时，出现上述情况broker会直接将消息丢弃

                  using var channel = await connectionManager.Connection.CreateChannelAsync();
                  await channel.BasicPublishAsync(exchange, routingKey, mandatory, basicProperties ?? new BasicProperties(), body, cancellationToken);
                  //开启发布消息确认模式
                  //_channel.ConfirmSelect();
                  //消息是否到达服务器
                  //bool publishStatus = _channel.WaitForConfirms();
              });
        return Task.CompletedTask;
    }

    public virtual BasicProperties CreateBasicProperties() => new();

    //public void Dispose()
    //{
    //    Dispose(true);
    //    GC.SuppressFinalize(this);
    //}

    //protected virtual void Dispose(bool disposing)
    //{
    //    if (disposing)
    //    {
    //        if (_channel != null)
    //            _channel.Dispose();
    //    }
    //}
}
