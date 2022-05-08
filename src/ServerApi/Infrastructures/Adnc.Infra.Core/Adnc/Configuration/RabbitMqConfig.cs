namespace Adnc.Infra.Core.Configuration;

public class RabbitMqConfig
{
    public string HostName { get; set; }
    public string VirtualHost { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
}