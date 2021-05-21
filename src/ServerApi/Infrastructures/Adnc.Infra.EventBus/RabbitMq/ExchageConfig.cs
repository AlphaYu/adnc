namespace Adnc.Infra.EventBus.RabbitMq
{
    public class ExchageConfig
    {
        public string Name { get; set; }
        public ExchangeType Type { get; set; }
        public string DeadExchangeName { get; set; }
    }
}