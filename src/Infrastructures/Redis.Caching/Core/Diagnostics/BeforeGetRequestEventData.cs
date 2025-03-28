namespace Adnc.Infra.Redis.Caching.Core.Diagnostics;

public class BeforeGetRequestEventData : EventData
{
    public BeforeGetRequestEventData(string cacheType, string name, string operation, string[] cacheKeys, System.TimeSpan? expiration = null)
        : base(cacheType, name, operation)
    {
        CacheKeys = cacheKeys;
        Expiration = expiration;
    }

    public string[] CacheKeys { get; set; }

    public TimeSpan? Expiration { get; set; }
}
