using Microsoft.Extensions.Configuration;
using Autofac;
using Adnc.Infra.Caching.Configurations;
using Adnc.Infra.Caching.StackExchange;
using Adnc.Infra.Caching.Core.Serialization;
using Adnc.Infra.Core.Interceptor;
using Adnc.Infra.Caching.Interceptor.Castle;

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
            builder.RegisterType<DefaultRedisProvider>().As<IRedisProvider, ICacheProvider, IDistributedLocker>().SingleInstance();
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                       .Where(t => t.IsAssignableTo<ICachingSerializer>())
                       .AsImplementedInterfaces()
                       .SingleInstance();
            builder.RegisterType<CachingInterceptor>().InstancePerLifetimeScope();
        }
    }
}
