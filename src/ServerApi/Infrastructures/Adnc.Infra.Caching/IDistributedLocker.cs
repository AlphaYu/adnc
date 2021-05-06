using System;
using System.Threading.Tasks;

namespace Adnc.Infra.Caching
{
    public interface IDistributedLocker
    {
        Task<bool> LockAsync(string cacheKey, string cacheValue, TimeSpan? expiration = null);
        Task<bool> SafedUnLockAsync(string cacheKey, string cacheValue);

        bool Lock(string cacheKey, string cacheValue, TimeSpan? expiration = null);
        bool SafedUnLock(string cacheKey, string cacheValue);
    }
}
