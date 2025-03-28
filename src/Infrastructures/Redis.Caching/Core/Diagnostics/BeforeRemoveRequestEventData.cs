namespace Adnc.Infra.Redis.Caching.Core.Diagnostics;

public class BeforeRemoveRequestEventData : EventData
{
    public BeforeRemoveRequestEventData(string cacheType, string name, string operation, string[] cacheKeys)
        : base(cacheType, name, operation)
    {
        CacheKeys = cacheKeys;
    }

    public string[] CacheKeys { get; set; }
}