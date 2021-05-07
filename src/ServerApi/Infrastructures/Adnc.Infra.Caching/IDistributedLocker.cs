using System;
using System.Threading.Tasks;

namespace Adnc.Infra.Caching
{
    public interface IDistributedLocker
    {
        Task<(bool Success, string LockValue)> LockAsync(string cacheKey, int timeoutSeconds = 5, bool autoDelay = true);

        Task<bool> SafedUnLockAsync(string cacheKey, string cacheValue);

        (bool Success, string LockValue) Lock(string cacheKey, int timeoutSeconds = 5, bool autoDelay = true);

        bool SafedUnLock(string cacheKey, string cacheValue);
    }
}
