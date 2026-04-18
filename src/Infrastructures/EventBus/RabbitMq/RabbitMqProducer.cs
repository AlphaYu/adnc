using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Adnc.Infra.EventBus.RabbitMq;

public class RabbitMqProducer(IConnectionManager connectionManager, ILogger<RabbitMqProducer> logger)
{
    /*
    /// <summary>
    /// Simple queue; does not route through an exchange.
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
                  // When mandatory is true, if the exchange cannot find a suitable queue for the message based on its type and routingKey,
                  // the broker will call basic.return to return the message to the producer.
                  // When mandatory is false, the broker silently drops the message in that case.

                  using var channel = await connectionManager.Connection.CreateChannelAsync();
                  await channel.BasicPublishAsync(exchange, routingKey, mandatory, basicProperties ?? new BasicProperties(), body, cancellationToken);
                  // Enable publisher confirms
                  //_channel.ConfirmSelect();
                  // Whether the message reached the server
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
