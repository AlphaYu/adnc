namespace Adnc.Infra.EventBus.RabbitMq;

/// <summary>
/// Queue configuration
/// </summary>
public class QueueConfig
{
    /// <summary>
    /// Queue name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dead-letter queue name; requires the dead-letter exchange to be configured first
    /// </summary>
    public string DeadQueueName
    {
        get { return $"dead-letter-{Name}"; }
    }

    /// <summary>
    /// Whether to make the queue durable
    /// </summary>
    public bool Durable { get; set; }

    /// <summary>
    /// exclusive: whether the queue is exclusive; has two effects:
    /// 1. Whether the queue is auto-deleted when the connection (connection.close()) is closed.
    /// 2. Whether the queue is private; if not exclusive, two consumers can access the same queue without issue.
    ///    If exclusive, the queue is locked to the current channel; other channels cannot access it and will throw an exception.
    ///    Typically set to true when only one consumer should consume from the queue.
    /// </summary>
    public bool Exclusive { get; set; }

    /// <summary>
    /// Whether to auto-delete the queue when the last consumer disconnects
    /// </summary>
    public bool AutoDelete { get; set; }

    /// <summary>
    /// Extended queue argument configuration:
    /// x-dead-letter-exchange: sets the DLX (dead-letter exchange) for this queue
    /// x-dead-letter-routing-key: sets the routing key for the DLX; the DLX uses this to route dead-letter messages to the appropriate queue
    /// x-message-ttl: sets the message TTL (time-to-live) in milliseconds
    /// </summary>
    public IDictionary<string, object?>? Arguments { get; set; }

    /// <summary>
    /// Whether to enable auto-acknowledgement
    /// </summary>
    public bool AutoAck { get; set; }

    /// <summary>
    /// When global=true, applies to all consumers on the current channel; otherwise only affects consumers created after the setting is applied
    /// </summary>
    public bool Global { get; set; }

    /// <summary>
    /// Takes effect only when manual acknowledgement is enabled.
    /// Whether to acknowledge multiple messages at once.
    /// </summary>
    public bool AckMultiple { get; set; }

    /// <summary>
    /// Takes effect only when manual acknowledgement is enabled.
    /// requeue = true: re-queue the message.
    /// requeue = false: route to the dead-letter queue if configured, otherwise discard.
    /// </summary>
    public bool RejectRequeue { get; set; }

    /// <summary>
    /// Takes effect only when manual acknowledgement is enabled.
    /// Limits to one unacknowledged message per consumer at a time; no new messages are sent until the current one is acknowledged.
    /// </summary>
    public ushort PrefetchCount { get; set; }
}
