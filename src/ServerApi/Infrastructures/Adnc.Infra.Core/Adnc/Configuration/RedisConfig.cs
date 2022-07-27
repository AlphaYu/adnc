namespace Adnc.Infra.Core.Configuration;

/// <summary>
/// RedisConfig配置
/// </summary>
public class RedisConfig
{
    public const string Name = "Redis";
    public string Provider { get; set; } = "StackExchange";
    public int MaxRdSecond { get; set; }
    public bool EnableLogging { get; set; }
    public bool EnableBloomFilter { get; set; }
    public int LockMs { get; set; }
    public int SleepMs { get; set; }
    public Dbconfig Dbconfig { get; set; } = default!;
}

public class Dbconfig
{
    public string ConnectionString { get; set; } = string.Empty;
    public bool ReadOnly { get; set; }
}