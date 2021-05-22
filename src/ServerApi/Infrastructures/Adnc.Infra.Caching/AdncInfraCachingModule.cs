using Adnc.Infra.Caching.Configurations;
using Adnc.Infra.Caching.Core.Serialization;
using Adnc.Infra.Caching.Interceptor.Castle;
using Adnc.Infra.Caching.StackExchange;
using Adnc.Infra.Core.Interceptor;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Adnc.Infra.Caching
{
    /// <summary>
    /// 注册Caching
    /// </summary>
    public class AdncInfraCachingModule : Autofac.Module
    {
        private readonly CacheOptions _cacheOptions;

        public AdncInfraCachingModule(IConfigurationSection redisSection)
        {
            _cacheOptions = redisSection.Get<CacheOptions>();
        }

        public AdncInfraCachingModule(CacheOptions cacheOptions)
        {
            _cacheOptions = cacheOptions;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_cacheOptions);
            builder.RegisterType<DefaultDatabaseProvider>().As<IRedisDatabaseProvider>().SingleInstance();
            builder.RegisterType<DefaultCachingKeyGenerator>().As<ICachingKeyGenerator>().SingleInstance();
            builder.RegisterType<DefaultRedisProvider>().As<IRedisProvider, IDistributedLocker, ICacheProvider>().SingleInstance();
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                       .Where(t => t.IsAssignableTo<ICachingSerializer>())
                       .As<ICachingSerializer>()
                       .SingleInstance();
            builder.RegisterType<CachingInterceptor>().InstancePerLifetimeScope();
        }
    }
}