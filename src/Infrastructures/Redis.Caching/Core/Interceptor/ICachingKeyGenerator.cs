namespace Adnc.Infra.Redis.Caching.Core.Interceptor
{
    /// <summary>
    /// Adnc.Infra.Redis key generator.
    /// </summary>
    public interface ICachingKeyGenerator
    {
        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <returns>The cache key.</returns>
        /// <param name="methodInfo">Method info.</param>
        /// <param name="args">Arguments.</param>
        /// <param name="prefix">Prefix.</param>
        string GetCacheKey(MethodInfo methodInfo, object[] args, string prefix);

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <returns>The cache key.</returns>
        /// <param name="methodInfo">Method info.</param>
        /// <param name="args">Arguments.</param>
        /// <param name="prefix">Prefix.</param>
        string[] GetCacheKeys(MethodInfo methodInfo, object[] args, string prefix);

        /// <summary>
        /// Gets the cache key prefix.
        /// </summary>
        /// <returns>The cache key prefix.</returns>
        /// <param name="methodInfo">Method info.</param>
        /// <param name="prefix">Prefix.</param>
        string GetCacheKeyPrefix(MethodInfo methodInfo, string prefix);
    }
}