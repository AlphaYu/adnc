namespace Adnc.Infra.Redis.Caching.Core.Preheater;

public interface ICachePreheatable
{
    /// <summary>
    /// 预热缓存
    /// </summary>
    /// <returns></returns>
    Task PreheatAsync();
}
