namespace Adnc.Infra.Redis
{
    public interface IDistributedLocker
    {
        /// <summary>
        /// 获取分布式锁
        /// </summary>
        /// <param name="cacheKey">cacheKey.</param>
        /// <param name="timeoutSeconds">锁定时间</param>
        /// <param name="autoDelay">是否自己续期</param>
        /// <returns>Success 获取锁的状态，LockValue锁的版本号</returns>
        Task<(bool Success, string LockValue)> LockAsync(string cacheKey, int timeoutSeconds = 5, bool autoDelay = false);

        /// <summary>
        /// 安全解锁
        /// </summary>
        /// <param name="cacheKey">cacheKey.</param>
        /// <param name="cacheValue">版本号</param>
        /// <returns></returns>
        Task<bool> SafedUnLockAsync(string cacheKey, string cacheValue);

        /// <summary>
        /// 获取分布式锁
        /// </summary>
        /// <param name="cacheKey">cacheKey.</param>
        /// <param name="timeoutSeconds">锁定时间</param>
        /// <param name="autoDelay">是否自己续期</param>
        /// <returns>Success 获取锁的状态，LockValue锁的版本号</returns>
        (bool Success, string LockValue) Lock(string cacheKey, int timeoutSeconds = 5, bool autoDelay = false);

        /// <summary>
        /// 安全解锁
        /// </summary>
        /// <param name="cacheKey">cacheKey.</param>
        /// <param name="cacheValue">版本号</param>
        /// <returns></returns>
        bool SafedUnLock(string cacheKey, string cacheValue);
    }
}