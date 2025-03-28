namespace Adnc.Infra.Redis.Caching.Core.Diagnostics;

using System;
using System.Collections.Generic;

public class BeforeSetRequestEventData(string cacheType, string name, string operation, IDictionary<string, object> dict, TimeSpan expiration) : EventData(cacheType, name, operation)
{
    public IDictionary<string, object> Dict { get; set; } = dict;

    public TimeSpan Expiration { get; set; } = expiration;
}
