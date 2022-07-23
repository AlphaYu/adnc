using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Configurations;
using Adnc.Infra.Caching.Core;
using Adnc.Infra.Caching.Core.Serialization;
using Adnc.Infra.Caching.Interceptor.Castle;
using StackExchangeProvider = Adnc.Infra.Caching.StackExchange;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraCaching(this IServiceCollection services, IConfigurationSection redisSection)
    {
        if (services.HasRegistered(nameof(AddAdncInfraCaching)))
            return services;

        services
            .Configure<CacheOptions>(redisSection)
            .Configure<RedisConfig>(redisSection)
            .AddSingleton<ICachingKeyGenerator, DefaultCachingKeyGenerator>()
            .AddScoped<CachingInterceptor>()
            .AddScoped<CachingAsyncInterceptor>();
        var serviceType = typeof(ICachingSerializer);
        var implementations = serviceType.Assembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        implementations.ForEach(implementationType => services.AddSingleton(serviceType, implementationType));

        var redisConfig = redisSection.Get<RedisConfig>();
        switch (redisConfig.Provider)
        {
            case CachingConstValue.Provider.StackExchange:
                AddAdncStackExchange(services);
                break;
            case CachingConstValue.Provider.ServiceStack:
                break;
            case CachingConstValue.Provider.FreeRedis:
                break;
            case CachingConstValue.Provider.CSRedis:
                break;
            default:
                throw new NotSupportedException(nameof(redisConfig.Provider));
        }
        return services;
    }

    public static IServiceCollection AddAdncStackExchange(IServiceCollection services)
    {
        return
            services
            .AddSingleton<StackExchangeProvider.DefaultDatabaseProvider>()
            .AddSingleton<StackExchangeProvider.DefaultRedisProvider>()
            .AddSingleton<IRedisProvider>(x => x.GetRequiredService<StackExchangeProvider.DefaultRedisProvider>())
            .AddSingleton<IDistributedLocker>(x => x.GetRequiredService<StackExchangeProvider.DefaultRedisProvider>())
            .AddSingleton<ICacheProvider, StackExchangeProvider.CachingProvider>();
    }
}