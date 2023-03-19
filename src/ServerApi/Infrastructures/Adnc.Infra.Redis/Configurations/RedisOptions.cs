namespace Adnc.Infra.Redis.Configurations;

/// <summary>
/// RedisConfig Configurations
/// </summary>
public class RedisOptions
{
    public string Provider { get; set; } = "StackExchange";
    public bool EnableBloomFilter { get; set; }
    public string SerializerName { get; set; } = string.Empty;
    public DBOptions Dbconfig { get; set; } = default!;
}