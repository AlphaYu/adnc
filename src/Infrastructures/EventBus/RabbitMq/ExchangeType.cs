namespace Adnc.Infra.EventBus.RabbitMq;

public enum ExchangeType
{
    // Publish-subscribe mode
    Fanout,

    // Routing mode
    Direct,

    // Wildcard/topic mode
    Topic
}
