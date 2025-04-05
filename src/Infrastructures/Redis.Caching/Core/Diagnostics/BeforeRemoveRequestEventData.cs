namespace Adnc.Infra.Redis.Caching.Core.Diagnostics;

public class BeforeRemoveRequestEventData(string cacheType, string name, string operation, string[] cacheKeys) : EventData(cacheType, name, operation)
{
    public string[] CacheKeys { get; set; } = cacheKeys;
}
