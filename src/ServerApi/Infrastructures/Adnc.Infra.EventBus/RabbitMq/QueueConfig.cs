namespace Adnc.Infra.EventBus.RabbitMq
{
    /// <summary>
    /// 队列配置信息
    /// </summary>
    public class QueueConfig
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 死信队列名称,需要先配置死信交互机
        /// </summary>
        public string DeadQueueName
        {
            get { return $"dead-letter-{Name}"; }
        }

        /// <summary>
        /// 是否持久化队列
        /// </summary>
        public bool Durable { get; set; }

        /// <summary>
        /// exclusive exclusive：是否排外的，有两个作用
        /// 1、当连接关闭时connection.close()该队列是否会自动删除；
        /// 2、该队列是否是私有的private，如果不是排外的，可以使用两个消费者都访问同一个队列，没有任何问题；
        ///   如果是排外的，会对当前队列加锁，其他通道channel是不能访问的，如果强制访问会报异常
        ///  一般等于true的话用于一个队列只能有一个消费者来消费的场景
        /// </summary>
        public bool Exclusive { get; set; }

        /// <summary>
        /// 是否自动删除，当最后一个消费者断开连接之后队列是否自动被删除
        /// </summary>
        public bool AutoDelete { get; set; }

        /// <summary>
        /// 队列扩展参数配置
        /// x-dead-letter-exchange 设置当前队列的DLX(死信交机)
        /// x-dead-letter-routing-key 设置DLX的路由key，DLX会根据该值去找到死信消息存放的队列
        /// x-message-ttl 设置消息的存活时间，即过期时间(毫秒)
        /// </summary>
        public IDictionary<string, object>? Arguments { get; set; }

        /// <summary>
        /// 是否开启自动确认
        /// </summary>
        public bool AutoAck { get; set; }

        /// <summary>
        /// glotal=true时表示在当前channel上所有的consumer都生效，否则只对设置了之后新建的consumer生效
        /// </summary>
        public bool Global { get; set; }

        /// <summary>
        /// 开启手动确认后才生效
        /// 是否一次可以确认多条
        /// </summary>
        public bool AckMultiple { get; set; }

        /// <summary>
        /// 开启手动确认后才生效
        /// requeue = true,重新放回队列
        /// requeue = false,如果配置死信队列，会转义到死信队列,没有则丢弃。
        /// </summary>
        public bool RejectRequeue { get; set; }

        /// <summary>
        /// 开启手动确认后才生效
        /// 每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
        /// </summary>
        public ushort PrefetchCount { get; set; }
    }
}