namespace Adnc.Infra.Consul.Configuration;

public class ConsulOptions
{
    public string ConsulUrl { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string HealthCheckUrl { get; set; } = string.Empty;
    public int HealthCheckIntervalInSecond { get; set; }
    public string[] ServerTags { get; set; } = [];
    public string ConsulKeyPath { get; set; } = string.Empty;
    public int DeregisterCriticalServiceAfter { get; set; }
    public int Timeout { get; set; }
    public string Token { get; set; } = string.Empty;
}
