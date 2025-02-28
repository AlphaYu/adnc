namespace Adnc.Infra.Redis.Caching.Core.Interceptor
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public class CachingParamAttribute : Attribute
    {
        public CachingParamAttribute()
        {
        }
    }
}