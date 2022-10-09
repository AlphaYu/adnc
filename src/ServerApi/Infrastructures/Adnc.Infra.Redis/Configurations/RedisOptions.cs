namespace Adnc.Infra.Redis.Configurations;

/// <summary>
/// RedisConfig配置
/// </summary>
public class RedisOptions
{
    public const string Name = "Redis";
    public string Provider { get; set; } = "StackExchange";
    public bool EnableBloomFilter { get; set; }
    public string SerializerName { get; set; }
    public int MaxRdSecond { get; set; }
    public bool EnableLogging { get; set; }
    public int LockMs { get; set; }
    public int SleepMs { get; set; }
    public DBOptions DBOptions { get; set; } = default!;
}