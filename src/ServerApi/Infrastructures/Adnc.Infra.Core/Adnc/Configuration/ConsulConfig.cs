namespace Adnc.Infra.Core.Configuration;

public class ConsulConfig
{
    public string ConsulUrl { get; set; }
    public string ServiceName { get; set; }
    public string HealthCheckUrl { get; set; }
    public int HealthCheckIntervalInSecond { get; set; }
    public string[] ServerTags { get; set; }
    public string ConsulKeyPath { get; set; }
    public int DeregisterCriticalServiceAfter { get; set; }
    public int Timeout { get; set; }
}