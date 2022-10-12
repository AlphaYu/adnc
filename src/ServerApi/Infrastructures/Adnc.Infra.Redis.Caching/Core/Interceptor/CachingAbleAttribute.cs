namespace Adnc.Infra.Redis.Caching.Core.Interceptor
{
    /// <summary>
    /// Adnc.Infra.Redis able attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAbleAttribute : CachingInterceptorAttribute
    {
        /// <summary>
        /// Gets or sets the expiration. The default value is 30 second.
        /// </summary>
        /// <value>The expiration.</value>
        public int Expiration { get; set; } = 30;
    }
}