namespace Adnc.Infra.Core.Configuration;

public class ConsulConfig
{
    public const string Name = "Consul";

    public string ConsulUrl { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string HealthCheckUrl { get; set; } = string.Empty;
    public int HealthCheckIntervalInSecond { get; set; } = default;
    public string[] ServerTags { get; set; } = Array.Empty<string>();
    public string ConsulKeyPath { get; set; } = string.Empty;
    public int DeregisterCriticalServiceAfter { get; set; } = default;
    public int Timeout { get; set; } = default;
}