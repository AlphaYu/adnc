namespace Adnc.Infra.Redis.Caching.Core.Diagnostics;

public class BeforeExistsRequestEventData(string cacheType, string name, string operation, string cacheKey) : EventData(cacheType, name, operation)
{
    public string CacheKey { get; set; } = cacheKey;
}
