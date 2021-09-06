using System.Threading.Tasks;

namespace Adnc.Application.Shared.Caching
{
    public interface ICacheService
    {
        /// <summary>
        /// 预热缓存
        /// </summary>
        /// <returns></returns>
        Task PreheatAsync();
    }
}
