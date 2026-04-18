namespace Adnc.Infra.Redis;

public interface IDistributedLocker
{
    /// <summary>
    /// Acquires a distributed lock.
    /// </summary>
    /// <param name="cacheKey">cacheKey.</param>
    /// <param name="timeoutSeconds">Lock timeout in seconds</param>
    /// <param name="autoDelay">Whether to auto-renew the lock</param>
    /// <returns>Success: lock acquisition status; LockValue: lock version token</returns>
    Task<(bool Success, string LockValue)> LockAsync(string cacheKey, int timeoutSeconds = 5, bool autoDelay = false);

    /// <summary>
    /// Safely releases the distributed lock.
    /// </summary>
    /// <param name="cacheKey">cacheKey.</param>
    /// <param name="cacheValue">Version token</param>
    /// <returns></returns>
    Task<bool> SafedUnLockAsync(string cacheKey, string cacheValue);

    /// <summary>
    /// Acquires a distributed lock.
    /// </summary>
    /// <param name="cacheKey">cacheKey.</param>
    /// <param name="timeoutSeconds">Lock timeout in seconds</param>
    /// <param name="autoDelay">Whether to auto-renew the lock</param>
    /// <returns>Success: lock acquisition status; LockValue: lock version token</returns>
    (bool Success, string LockValue) Lock(string cacheKey, int timeoutSeconds = 5, bool autoDelay = false);

    /// <summary>
    /// Safely releases the distributed lock.
    /// </summary>
    /// <param name="cacheKey">cacheKey.</param>
    /// <param name="cacheValue">Version token</param>
    /// <returns></returns>
    bool SafedUnLock(string cacheKey, string cacheValue);
}
