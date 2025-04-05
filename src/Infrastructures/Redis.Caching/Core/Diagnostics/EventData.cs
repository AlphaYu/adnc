namespace Adnc.Infra.Redis.Caching.Core.Diagnostics;

public class EventData(string cacheType, string name, string operation)
{
    public string CacheType { get; set; } = cacheType;

    public string Name { get; set; } = name;

    public string Operation { get; set; } = operation;
}
