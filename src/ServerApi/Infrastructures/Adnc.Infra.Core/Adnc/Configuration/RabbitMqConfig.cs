namespace Adnc.Infra.Core.Configuration;

public class RabbitMqConfig
{
    public const string Name = "RabbitMq";
    public string HostName { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Port { get; set; }
}