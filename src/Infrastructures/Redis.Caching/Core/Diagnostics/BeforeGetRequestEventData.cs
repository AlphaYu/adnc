namespace Adnc.Infra.Redis.Caching.Core.Diagnostics;

public class BeforeGetRequestEventData(string cacheType, string name, string operation, string[] cacheKeys, TimeSpan? expiration = null) : EventData(cacheType, name, operation)
{
    public string[] CacheKeys { get; set; } = cacheKeys;

    public TimeSpan? Expiration { get; set; } = expiration;
}
