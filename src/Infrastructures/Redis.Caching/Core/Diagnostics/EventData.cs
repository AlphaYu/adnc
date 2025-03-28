namespace Adnc.Infra.Redis.Caching.Core.Diagnostics;

public class EventData
{
    public EventData(string cacheType, string name, string operation)
    {
        CacheType = cacheType;
        Name = name;
        Operation = operation;
    }

    public string CacheType { get; set; }

    public string Name { get; set; }

    public string Operation { get; set; }
}