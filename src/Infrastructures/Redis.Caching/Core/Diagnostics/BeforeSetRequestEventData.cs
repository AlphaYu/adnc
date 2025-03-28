namespace Adnc.Infra.Redis.Caching.Core.Diagnostics;

using System;
using System.Collections.Generic;

public class BeforeSetRequestEventData : EventData
{
    public BeforeSetRequestEventData(string cacheType, string name, string operation, IDictionary<string, object> dict, System.TimeSpan expiration)
        : base(cacheType, name, operation)
    {
        Dict = dict;
        Expiration = expiration;
    }

    public IDictionary<string, object> Dict { get; set; }

    public TimeSpan Expiration { get; set; }
}