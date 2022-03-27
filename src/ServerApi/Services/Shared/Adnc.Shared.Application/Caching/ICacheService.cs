namespace Adnc.Shared.Application.Caching;

public interface ICacheService
{
    /// <summary>
    /// 预热缓存
    /// </summary>
    /// <returns></returns>
    Task PreheatAsync();
}