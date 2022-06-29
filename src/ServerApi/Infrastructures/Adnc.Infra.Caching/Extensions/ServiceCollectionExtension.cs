using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Configurations;
using Adnc.Infra.Caching.Core.Serialization;
using Adnc.Infra.Caching.Interceptor.Castle;
using Adnc.Infra.Caching.StackExchange;

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
            .AddSingleton<IRedisDatabaseProvider, DefaultDatabaseProvider>()
            .AddSingleton<ICachingKeyGenerator, DefaultCachingKeyGenerator>()
            .AddSingleton<DefaultRedisProvider>()
            .AddSingleton<IRedisProvider>(x => x.GetRequiredService<DefaultRedisProvider>())
            .AddSingleton<IDistributedLocker>(x => x.GetRequiredService<DefaultRedisProvider>())
            .AddSingleton<ICacheProvider>(x => x.GetRequiredService<DefaultRedisProvider>())
            .AddScoped<CachingInterceptor>()
            .AddScoped<CachingAsyncInterceptor>()
            ;
        var serviceType = typeof(ICachingSerializer);
        var implementations = serviceType.Assembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        implementations.ForEach(implementationType => services.AddSingleton(serviceType, implementationType));

        return services;
    }
}