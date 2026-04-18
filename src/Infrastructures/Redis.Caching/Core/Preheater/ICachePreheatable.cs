namespace Adnc.Infra.Redis.Caching.Core.Preheater;

public interface ICachePreheatable
{
    /// <summary>
    /// Preheat the cache.
    /// </summary>
    /// <returns></returns>
    Task PreheatAsync();
}
