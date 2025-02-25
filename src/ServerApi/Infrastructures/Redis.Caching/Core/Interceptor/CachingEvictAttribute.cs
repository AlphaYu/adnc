namespace Adnc.Infra.Redis.Caching.Core.Interceptor
{
    using System;

    /// <summary>
    /// Adnc.Infra.Redis evict attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingEvictAttribute : CachingInterceptorAttribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether evict all cached values by cachekey prefix
        /// </summary>
        /// <remarks>
        /// This need to use with CacheKeyPrefix.
        /// </remarks>
        /// <value><c>true</c> if is all; otherwise, <c>false</c>.</value>
        //public bool IsAll { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether is before.
        /// </summary>
        /// <value><c>true</c> if is before; otherwise, <c>false</c>.</value>
        //public bool IsBefore { get; set; } = false;

        /// <summary>
        /// The cache keys
        /// </summary>
        public string[] CacheKeys { get; set; } = new string[] { };
    }
}